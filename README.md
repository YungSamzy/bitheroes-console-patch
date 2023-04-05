
# Bit Heroes Quest Patcher

A small project of mine that patches the game to show all Unity logs.


## Download

Just go to [Releases](https://github.com/YungSamzy/bitheroes-console-patch/releases/latest) to download the latest binaries...


## How this works

I first patched all ```Unity.Log``` functions in UnityEngine.CoreModule.dll. Then I called hooked into ```SteamLogin()``` in Assembly-CSharp.dll to call the console when the program logs into Steam.

Hook Example:
```csharp
public class SteamLogin : PlatformLogin
	{
		// Token: 0x06003FCB RID: 16331 RVA: 0x0011E6D0 File Offset: 0x0011C8D0
		public override void Login(UnityAction<float> onLoginCompleted, UnityAction<float> onLoginFailed)
		{
			WinConsole.Initialize(true);
			Console.Title = "Debug Console | Made by SamzyDev";
			Console.WriteLine("#### Made By SamzyDev ####");
			base.Login(onLoginCompleted, onLoginFailed);
			if (SteamManager.Initialized)
			{
				base.OnLoginCompleted(0.5f);
				return;
			}
			base.OnLoginFailed(0f);
		}
        ...
```

## Demo
![preview](https://github.com/YungSamzy/bitheroes-console-patch/raw/main/preview.gif)
