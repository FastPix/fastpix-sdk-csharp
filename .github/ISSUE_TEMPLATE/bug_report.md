---
name: Bug Report
about: Report a bug or unexpected behavior in the FastPix C# SDK
title: '[BUG] '
labels: ['bug', 'needs-triage']
assignees: ''
---

# Bug Report

Thank you for taking the time to report a bug with the FastPix C# SDK. To help us resolve your issue quickly and efficiently, please provide the following information:

## Description
**Clear and concise description of the bug:**
```
<!-- Please provide a detailed description of what you're experiencing -->
```

## Environment Information

### System Details
- **.NET Version:** [e.g., .NET 6.0, .NET 7.0, .NET 8.0]
- **Operating System:** [e.g., Windows 10, macOS 12.0, Ubuntu 20.04, etc.]
- **IDE:** [e.g., Visual Studio 2022, Rider, VS Code, etc.] (Optional but helpful)

### SDK Information
- **FastPix C# SDK Version:** [e.g., 1.0.3, 1.0.2, etc.]
- **C# Version:** [e.g., C# 10, C# 11, C# 12, etc.]
- **Framework:** [e.g., .NET 6.0, .NET 7.0, .NET 8.0, etc.]

## Reproduction Steps

1. **Setup Environment:**
   ```bash
   dotnet add package Fastpix
   ```

2. **Code to Reproduce:**
   ```csharp
   // Please provide a minimal, reproducible example
   using Fastpix;
   using Fastpix.Models.Components;

   var fastpix = new FastpixSDK(new FastpixSDK.Builder()
   {
       Security = new Security.Builder()
       {
           Username = "your-username",
           Password = "your-password"
       }.Build()
   });

   // Your code here that causes the issue
   ```

3. **Expected Behavior:**

    ```
    <!-- Describe what you expected to happen -->
    ```

4. **Actual Behavior:**

    ```
    <!-- Describe what actually happened -->
    ```

5. **Error Messages/Logs:**
   ```
   <!-- Paste any error messages, stack traces, or logs here -->
   ```

## Debugging Information

### Console Output
```
<!-- Paste the complete console output here -->
```

### Error Stack Traces
```csharp
// Complete stack trace for C# errors
System.Exception: Error message here
   at Fastpix.SomeMethod() in /path/to/file.cs:line 123
   at YourNamespace.YourClass.YourMethod() in /path/to/your/file.cs:line 45
```

### HTTP Requests
```http
# Raw HTTP request (remove sensitive headers and credentials)
POST /api/endpoint HTTP/1.1
Host: [FastPix API endpoint]
Authorization: Basic ***
Content-Type: application/json

<!-- Remove credentials and sensitive headers before pasting -->
```

### Screenshots
```
<!-- If applicable, please attach screenshots that help explain your issue -->
```

## Additional Context

### Configuration
```csharp
// Please share your SDK configuration (remove sensitive information)
var fastpix = new FastpixSDK(new FastpixSDK.Builder()
{
    Security = new Security.Builder()
    {
        Username = "***", // Redacted
        Password = "***"  // Redacted
    }.Build(),
    // Any other configuration options
});
```

### Workarounds
```
<!-- If you've found any workarounds, please describe them here -->
```

## Priority
Please indicate the priority of this bug:

- [ ] Critical (Blocks production use)
- [ ] High (Significant impact on functionality)
- [ ] Medium (Minor impact)
- [ ] Low (Nice to have)

## Checklist
Before submitting, please ensure:

- [ ] I have searched existing issues to avoid duplicates
- [ ] I have provided all required information
- [ ] I have tested with the latest SDK version
- [ ] I have removed any sensitive information (credentials, API keys, etc.)
- [ ] I have provided a minimal reproduction case
- [ ] I have checked the documentation

---

**Thank you for helping improve the FastPix C# SDK! 🚀**

