# Forms

> Components used to simplify the handling of form inputs in Unity.

[![npm](https://img.shields.io/npm/v/xyz.candycoded.forms)](https://www.npmjs.com/package/xyz.candycoded.form)

## Installation

### Direct Install

[Download latest `CandyCoded.Forms.unitypackage` or `CandyCoded.Forms.dll`](https://github.com/CandyCoded/Forms/releases)

### Unity Package Manager

<https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.0/manual/index.html>

#### Git

```json
{
  "dependencies": {
    "xyz.candycoded.forms": "https://github.com/CandyCoded/Forms.git#v1.0.0",
    ...
  }
}
```

#### Scoped UPM Registry

```json
{
  "dependencies": {
    "xyz.candycoded.forms": "1.0.0",
    ...
  },
  "scopedRegistries": [
    {
      "name": "forms",
      "url": "https://registry.npmjs.com",
      "scopes": ["xyz.candycoded"]
    }
  ]
}
```

## Usage

First create a class with the same fields as the form.

```csharp
public class Profile
{
    public bool active;
    public string firstName;
    public string lastName;
    public int age;
}
```

Then add a serialize field to your MonoBehaviour to store a reference to the form component.

```csharp
[SerializeField]
private Form _form;
```

Then populate the fields with exisiting values (if applicable).

```csharp
public void Start()
{

    _form.LoadFormValues(new Profile
    {
        active = true,
        firstName = "Scott",
        lastName = "Doxey",
        age = 36
    });

}
```

Data can also be loaded via a `Dictionary<string, object>` object.

```csharp
public void Start()
{

    _form.LoadFormRawValues(new Dictionary<string, object>
      {
          { "active", true },
          { "firstName", "Scott" },
          { "lastName", "Doxey" },
          { "age", 36 },
      });

}
```

Then, on submit, use `Newtonsoft.Json` to convert the values into a JSON object.

```csharp
public void Submit()
{

    Debug.Log(JsonConvert.SerializeObject(_form.GetFormValues<Profile>()));

}
```
