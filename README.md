# Omega Leo's Helper Library

[![Publish NuGet](https://github.com/omegaleo/HelperLib/actions/workflows/nuget-publish.yml/badge.svg)](https://github.com/omegaleo/HelperLib/actions/workflows/nuget-publish.yml)
[![Publish JetBrains Plugin](https://github.com/omegaleo/HelperLib/actions/workflows/jetbrains-publish.yml/badge.svg)](https://github.com/omegaleo/HelperLib/actions/workflows/jetbrains-publish.yml)

Lightweight utility library for C\# projects and game development helpers.

## Table of contents

- [Overview](#overview)
- [Features](#features)
- [Install](#install)
- [Documentation](#documentation)
- [Contributing](#contributing)
- [License](#license)
- [Socials](#socials)

## Overview

`HelperLib` contains small helpers and abstractions commonly useful in application and game development (helpers for collections, math, and utilities to keep game and engine code concise).

## Features

- Small, focused utilities for C\#
- Helpers that favor clarity and testability
- Game development specific helpers in the `OmegaLeo.HelperLib.Game` package
- Changelog generation utilities in the `OmegaLeo.HelperLib.Changelog` package
- **JetBrains Rider plugin** for displaying custom documentation attributes in Quick Documentation (Ctrl+Q)
- Lightweight and easy to integrate into existing projects
- Well-documented and maintained
- Open source under the AGPL v3 License

## Install

### NuGet Packages

Add the library to your project via NuGet:

```bash
dotnet add package OmegaLeo.HelperLib
dotnet add package OmegaLeo.HelperLib.Game
dotnet add package OmegaLeo.HelperLib.Changelog
dotnet add package OmegaLeo.HelperLib.Documentation
```

### JetBrains Rider Plugin

For enhanced IDE support with custom documentation attributes:

1. Open Rider
2. Go to **Settings** → **Plugins**
3. Search for "HelperLib Documentation Provider"
4. Click **Install**
5. Restart Rider

The plugin displays `[Documentation]` attributes in Quick Documentation (Ctrl+Q). [Learn more →](OmegaLeo.HelperLib.Jetbrains/README.md)
  
## Documentation
The documentation for this project can be found at [https://library.omegaleo.dev](https://library.omegaleo.dev).
This documentation is automatically generated from the code using the [OmegaLeo.HelperLib.Documentation package](https://www.nuget.org/packages/OmegaLeo.HelperLib.Documentation).

## Contributing
Contributions are welcome! Please see the [CONTRIBUTING.md](CONTRIBUTING.md) file for guidelines.

## License
This project is licensed under the AGPL v3 License. See the [LICENSE](LICENSE) file for details.

## Socials
[![Youtube](https://img.shields.io/youtube/channel/subscribers/UCsdYKC7EK4Z_cpgWt0YiO9Q?style=plastic&label=Youtube&color=red
)](https://youtube.com/@omegaleo) [![Twitch](https://img.shields.io/twitch/status/professorleo?style=plastic&logoColor=purple&color=purple
)](https://twitch.tv/professorleo) [![Discord](https://badgen.net/discord/online-members/Gp87EyBDvD?icon=discord)](https://discord.com/invite/Gp87EyBDvD)   
