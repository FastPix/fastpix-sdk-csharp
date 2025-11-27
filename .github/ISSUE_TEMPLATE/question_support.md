---
name: Question/Support
about: Ask questions or get help with the FastPix C# SDK
title: '[QUESTION] '
labels: ['question', 'needs-triage']
assignees: ''
---

# Question/Support

Thank you for reaching out! We're here to help you with the FastPix C# SDK. Please provide the following information:

## Question Type
- [ ] How to use a specific feature
- [ ] Integration help
- [ ] Configuration question
- [ ] Performance question
- [ ] Troubleshooting help
- [ ] Async/await patterns
- [ ] Error handling
- [ ] Other: _______________

## Question
**What would you like to know?**
```
<!-- Please provide a clear, specific question -->
```
## What You've Tried
**What have you already attempted to solve this?**

```csharp
// Please share any code you've tried
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

// Your attempted code here
```

## Current Setup
**Describe your current setup:**

### Environment
- **.NET Version:** [e.g., .NET 6.0, .NET 7.0, .NET 8.0]
- **C# Version:** [e.g., C# 10, C# 11, C# 12]
- **Operating System:** [e.g., Windows 10, macOS 12.0, Ubuntu 20.04, etc.]
- **FastPix C# SDK Version:** [e.g., 1.0.3, 1.0.2]
- **IDE:** [e.g., Visual Studio 2022, Rider, VS Code, etc.]

### Configuration
```csharp
// Your current SDK configuration (remove sensitive information)
var fastpix = new FastpixSDK(new FastpixSDK.Builder()
{
    Security = new Security.Builder()
    {
        Username = "***", // Redacted
        Password = "***"  // Redacted
    }.Build(),
    // Any other configuration
});
```

## Expected Outcome
**What are you trying to achieve?**
```
<!-- Describe your end goal -->
```
## Error Messages (if any)
```
<!-- If you're getting errors, paste them here -->
```

## Additional Context

### Use Case
**What are you building?**

- [ ] Web application (ASP.NET Core)
- [ ] Console application
- [ ] Desktop application
- [ ] Background service
- [ ] API service
- [ ] Other: _______________

### Project Details
- **Project Type:** [e.g., ASP.NET Core Web API, Console App, etc.]
- **Target Framework:** [e.g., .NET 6.0, .NET 7.0, .NET 8.0]

### Timeline
**When do you need this resolved?**

- [ ] ASAP (blocking development)
- [ ] This week
- [ ] This month
- [ ] No rush

### Resources Checked
**What resources have you already checked?**

- [ ] README.md
- [ ] Documentation
- [ ] Examples
- [ ] Stack Overflow
- [ ] GitHub Issues
- [ ] Other: _______________

## Priority
Please indicate the urgency:

- [ ] Critical (Blocking production deployment)
- [ ] High (Blocking development)
- [ ] Medium (Would like to know soon)
- [ ] Low (Just curious)

## Checklist
Before submitting, please ensure:

- [ ] I have provided a clear question
- [ ] I have described what I've tried
- [ ] I have included my current setup
- [ ] I have checked existing documentation
- [ ] I have provided sufficient context
- [ ] I have removed any sensitive information (credentials, API keys, etc.)

---

**We'll do our best to help you get unstuck! 🚀**

**For urgent issues, please also consider:**
- [FastPix Documentation](https://docs.fastpix.io/)
- [Stack Overflow](https://stackoverflow.com/questions/tagged/fastpix)
- [GitHub Discussions](https://github.com/FastPix/csharp-language-sdk-server-side/discussions)

