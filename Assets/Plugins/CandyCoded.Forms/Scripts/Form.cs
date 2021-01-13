// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CandyCoded.Forms
{

    [AddComponentMenu("CandyCoded / Forms / Form")]
    public class Form : MonoBehaviour
    {

        public FormEventObjectEvent FormChangedObject;

        public FormEventJSONEvent FormChangedJSON;

        public FormEventObjectEvent FormSubmittedObject;

        public FormEventJSONEvent FormSubmittedJSON;

        public Button submitButton;

        private EventSystem _eventSystem;

        private Form _parentForm;

        private void Awake()
        {

            _eventSystem = EventSystem.current;

            _parentForm = gameObject.GetComponentsInParent<Form>().FirstOrDefault(form => !form.Equals(this));

        }

        private void Update()
        {

            if (_eventSystem.currentSelectedGameObject == null || _parentForm != null)
            {
                return;
            }

            var selectable = _eventSystem.currentSelectedGameObject.GetComponent<Selectable>();

            var allSelectable = _eventSystem.currentSelectedGameObject.GetComponentInParent<Form>()
                .GetComponentsInChildren<Selectable>();

            if (!allSelectable.Contains(selectable))
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                HandleTabPress(selectable, allSelectable);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                HandleReturnPress();
            }

        }

        private void OnEnable()
        {

            foreach (var formField in GetChildFormFields())
            {

                formField.OnValueChanged.AddListener(HandleValueChanged);

            }

            if (submitButton)
            {

                submitButton.onClick.AddListener(HandleReturnPress);

            }

        }

        private void OnDisable()
        {

            foreach (var formField in GetChildFormFields())
            {

                formField.OnValueChanged.RemoveListener(HandleValueChanged);

            }

            if (submitButton)
            {

                submitButton.onClick.RemoveListener(HandleReturnPress);

            }

        }

        private void HandleTabPress(Selectable selectable, Selectable[] allSelectable)
        {

            var prevSelectable = selectable.FindSelectableOnUp() ?? allSelectable.Last();
            var nextSelectable = selectable.FindSelectableOnDown() ?? allSelectable.First();

            var next = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)
                ? prevSelectable
                : nextSelectable;

            _eventSystem.SetSelectedGameObject(next.gameObject, null);

        }

        private void HandleReturnPress()
        {

            FormSubmittedObject?.Invoke(GetFormRawValues());

            FormSubmittedJSON?.Invoke(ToJSON());

        }

        public IEnumerable<FormField> GetChildFormFields()
        {

            return gameObject.GetComponentsInChildren<FormField>()
                .Where(field => field.name != "" && field.parentForm.Equals(this));

        }

        public IEnumerable<Form> GetChildForms()
        {

            return gameObject.GetComponentsInChildren<Form>().Where(form => !form.Equals(this));

        }

        public Dictionary<string, object> GetFormRawValues()
        {

            return GetChildFormFields().ToDictionary(field => field.name, field => field.value);

        }

        public T GetFormValues<T>() where T : class, new()
        {

            var newObject = new T();
            var newObjectType = newObject.GetType();

            foreach (var item in GetFormRawValues())
            {
                newObjectType.GetField(item.Key)?.SetValue(newObject, item.Value);
                newObjectType.GetProperty(item.Key)?.SetValue(newObject, item.Value);
            }

            return newObject;

        }

        public string ToJSON()
        {

            return JsonConvert.SerializeObject(GetFormRawValues());

        }

        public string ToJSON<T>() where T : class, new()
        {

            return JsonConvert.SerializeObject(GetFormValues<T>());

        }

        public void LoadFromJSON(string json)
        {

            LoadFormRawValues(JsonConvert.DeserializeObject<Dictionary<string, object>>(json));

        }

        public void LoadFromJSON<T>(string json)
        {

            LoadFormValues(JsonConvert.DeserializeObject<T>(json));

        }

        public void LoadFormRawValues(Dictionary<string, object> values)
        {

            var formFields = GetChildFormFields();

            foreach (var value in values)
            {

                foreach (var formField in formFields)
                {

                    if (value.Key.Equals(formField.name))
                    {

                        formField.value = value.Value;

                    }

                }

            }

        }

        public void LoadFormValues<T>(T values)
        {

            var formFields = GetChildFormFields();

            foreach (var fieldInfo in values.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {

                foreach (var formField in formFields)
                {

                    if (fieldInfo.Name.Equals(formField.name))
                    {

                        formField.RemoveOnValueChangedEvent();
                        formField.value = fieldInfo.GetValue(values);
                        formField.AddOnValueChangedEvent();

                    }

                }

            }

            foreach (var propertyInfo in values.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {

                foreach (var formField in formFields)
                {

                    if (propertyInfo.Name.Equals(formField.name))
                    {

                        formField.value = propertyInfo.GetValue(values);

                    }

                }

            }

        }

        private void HandleValueChanged()
        {

            FormChangedObject?.Invoke(GetFormRawValues());

            FormChangedJSON?.Invoke(ToJSON());

        }

        [Serializable]
        public class FormEventObjectEvent : UnityEvent<Dictionary<string, object>>
        {

        }

        [Serializable]
        public class FormEventJSONEvent : UnityEvent<string>
        {

        }

    }

}
