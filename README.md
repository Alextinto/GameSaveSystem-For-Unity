# GameSaveSystem For Unity
Free and Open Source save system dedicated for Unity games.

A modular, async save system with pluggable serialization, encryption, and storage backends. Save complex game objects or simple key-value pairs with zero scene setup.

## Features
- **Modular workflow**: Serialization → Encryption → Storage. Swap any module to change how your save system works!
- **Two save modes**: Save complex C# objects/classes with `SaveManager` or simple key-value pairs with `ValuesManager` (like `PlayerPrefs`!)
- **Create your own modules**: Extend `Serializer`, `Encrypter`, or `Storage` base classes to customize your system
- **Plug and play**: Auto-initializes at runtime. No GameObjects, no MonoBehaviours, no scene setup required!
- **Fully async API**: All save, load, and delete operations are asynchronous

## Getting Started
### Prerequisites
- Unity 2022.3 or later

### Download
- Clone the project:

```
git clone https://github.com/Alextinto/GameSaveSystem-For-Unity.git
```

- [Download the latest release](https://github.com/Alextinto/GameSaveSystem-For-Unity/releases) *(Work in Progress)*
- [Download from Unity Asset Store](https://assetstore.unity.com/) *(Work in Progress)*

## Documentation
> *Work in Progress*

## License
[Apache License 2.0](LICENSE) © [Kibobyte](https://kibobyte.com)
