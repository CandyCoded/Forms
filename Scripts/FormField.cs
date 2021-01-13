// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CandyCoded.Forms
{

    [AddComponentMenu("CandyCoded / Forms / Form Field")]
    public class FormField : MonoBehaviour
    {

        private const string FIELD_NAME_PREFIX = "Field_";

        private static int _fieldCount;

#pragma warning disable CS0649
        [SerializeField]
        private string _name;
#pragma warning restore CS0649

        public UnityEvent OnValueChanged;

        private InputField _inputField;

        private Toggle _toggleField;

        private Dropdown _dropdownField;

        private Slider _sliderField;

        private Form _parentForm;

        private object _value;

        public Form parentForm
        {
            get
            {
                if (_parentForm == null)
                {
                    _parentForm = gameObject.GetComponentInParent<Form>();
                }

                return _parentForm;
            }
        }

        public new string name
        {
            get
            {
                if (_name == "")
                {

                    _name = $"{FIELD_NAME_PREFIX}{++_fieldCount}";

                }

                return _name;
            }
        }

        public object value
        {
            get
            {

                if (_inputField)
                {

                    return _inputField.text;

                }

                if (_toggleField)
                {

                    return _toggleField.isOn;

                }

                if (_dropdownField)
                {

                    return _dropdownField.value;

                }

                if (_sliderField)
                {

                    return _sliderField.value;

                }

                return _value;

            }
            set
            {
                if (_inputField)
                {

                    _inputField.text = value.ToString();

                }

                if (_toggleField)
                {

                    if (bool.TryParse(value.ToString(), out var valueBool))
                    {

                        _toggleField.isOn = valueBool;

                    }

                }

                if (_dropdownField)
                {

                    if (int.TryParse(value.ToString(), out var valueInt))
                    {

                        _dropdownField.value = valueInt;

                    }

                }

                if (_sliderField)
                {

                    if (float.TryParse(value.ToString(), out var valueFloat))
                    {

                        _sliderField.value = valueFloat;

                    }

                }

                _value = value;

            }
        }

        public void OnEnable()
        {

            gameObject.TryGetComponent(out _inputField);

            gameObject.TryGetComponent(out _toggleField);

            gameObject.TryGetComponent(out _dropdownField);

            gameObject.TryGetComponent(out _sliderField);

            AddOnValueChangedEvent();

        }

        public void OnDisable()
        {

            RemoveOnValueChangedEvent();

        }

        public void SetStringValue(string value)
        {

            _value = value;

        }

        public void SetIntValue(int value)
        {

            _value = value;

        }

        public void SetFloatValue(float value)
        {

            _value = value;

        }

        public void SetBoolValue(bool value)
        {

            _value = value;

        }

        public void AddOnValueChangedEvent()
        {

            if (_inputField)
            {

                _inputField.onValueChanged.AddListener(OnValueChangedEvent);

            }

            if (_toggleField)
            {

                _toggleField.onValueChanged.AddListener(OnValueChangedEvent);

            }

            if (_dropdownField)
            {

                _dropdownField.onValueChanged.AddListener(OnValueChangedEvent);

            }

            if (_sliderField)
            {

                _sliderField.onValueChanged.AddListener(OnValueChangedEvent);

            }

        }

        public void RemoveOnValueChangedEvent()
        {

            if (_inputField)
            {

                _inputField.onValueChanged.RemoveListener(OnValueChangedEvent);

            }

            if (_toggleField)
            {

                _toggleField.onValueChanged.RemoveListener(OnValueChangedEvent);

            }

            if (_dropdownField)
            {

                _dropdownField.onValueChanged.RemoveListener(OnValueChangedEvent);

            }

            if (_sliderField)
            {

                _sliderField.onValueChanged.RemoveListener(OnValueChangedEvent);

            }

        }

        private void OnValueChangedEvent(string _)
        {

            OnValueChanged?.Invoke();

        }

        private void OnValueChangedEvent(bool _)
        {

            OnValueChanged?.Invoke();

        }

        private void OnValueChangedEvent(float _)
        {

            OnValueChanged?.Invoke();

        }

        private void OnValueChangedEvent(int _)
        {

            OnValueChanged?.Invoke();

        }

    }

}
