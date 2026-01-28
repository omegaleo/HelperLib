# Omega Leo's Helper Library - Changelog
Package to help with Changelog generation by providing a new Changelog attribute and a tool to extract a dictionary of the changelogs or the markdown for CHANGELOG.md

Example attribute usage:
```cs
[Changelog("1.0.0", "This is a test")]
[Changelog("v1.0.1", "This is another test")]
public class Test
{
    
}
```