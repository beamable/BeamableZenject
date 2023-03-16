# Beamable and Zenject

[Zenject](https://github.com/modesttree/Zenject) is a powerful Dependency Injection (DI) tool for Unity. Beamable also uses an [internal DI system](https://docs.beamable.com/docs/player-centric-api-dependency-injection). This package integrates Beamable's DI system into the Zenject Installer format so you can continue to use Zenject while taking full advantage of Beamable.

## Getting Started

Before you get started, you will need a Unity project with Zenject installed. Beamable must also be installed. 
In the Unity Package Manager, [add a package via git-link](https://docs.unity3d.com/Manual/upm-ui-giturl.html). for com.beamable.solana and use the following git-link.

https://github.com/beamable/BeamableZenject.git?path=/Packages/com.beamable.zenject#1.0.0
Note: the end of the link includes the version number. You view the available versions by looking at this repositories git tags.

Using UPM, import the demo project to see Zenject in action.

## Usage

If you have any Zenject container, you can bind a `BeamContext` to that container. 
```csharp
var ctx = BeamContext.Default;
Container.BindBeamableContext(ctx);
```

There is a scriptable object installer called `BeamableInstaller` available that will automatically install a `BeamContext` into a Zenject container. This is how the sample works.

Once Beamable has been installed in a Zenject container, any of the Beamable services registered through Beamable's internal DI system can be accessed with Zenject's injection, via the `[Zenject]` attribute, or through construction. 

Look at the `ZenjectExampleBehaviour` for an example. Here is an except.
```csharp

public class ZenjectExampleBehaviour : MonoBehaviour
{
    [Header("Scene references")]
    public TextMeshProUGUI dbidLabel;
    public TextMeshProUGUI currencyLabel;
    
    [Inject]
    private BeamContext ctx;
    
    [Inject]
    private PlayerInventory inventory;
    
    async void Start()
    {
        dbidLabel.text = $"DBID: (loading)";
        
        // only need this because I don't have a "loading scene"
        await ctx.OnReady; // I wish we could enforce this timing into the Zenject cycle :/ 

        dbidLabel.text = $"DBID: {ctx.PlayerId}";
        inventory.GetCurrencies().OnDataUpdated += UpdateDisplay;
    }

    void UpdateDisplay(List<PlayerCurrency> currencies)
    {
        Debug.Log("updating display");
        var newText = string.Join("\n", currencies.Select(curr => $"{curr.CurrencyId}={curr.Amount}"));
        currencyLabel.text = newText;
    }

}
```