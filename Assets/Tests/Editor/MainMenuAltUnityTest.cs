using NUnit.Framework;
using Altom.AltUnityDriver;

public class MainMenuAltUnityTest
{
    public AltUnityDriver AltUnityDriver;
    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        AltUnityDriver =new AltUnityDriver();
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        AltUnityDriver.Stop();
    }

    [Test]
    public void FirstCountingGameShouldLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME,"CountingGameButton").Tap();
        
        AltUnityDriver.FindObject(By.NAME, "C1").Tap();
        AltUnityDriver.WaitForCurrentSceneToBe("Counting Game");
    }

    [Test]
    public void SecondCountingGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "CountingGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "C2").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void ThirdCountingGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "CountingGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "C3").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void FourthCountingGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "CountingGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "C4").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void FifthCountingGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "CountingGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "C5").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void SixthCountingGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "CountingGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "C6").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void FirstAdditionGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "AdditionGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "A1").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void SecondAdditionGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "AdditionGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "A2").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void ThirdAdditionGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "AdditionGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "A3").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void FourthAdditionGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "AdditionGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "A4").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void FifthAdditionGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "AdditionGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "A5").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void SixthAdditionGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "AdditionGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "A6").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void FirstSubGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "SubGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "S1").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void SecondSubGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "SubGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "S2").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void ThirdSubGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "SubGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "S3").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void FourthSubGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "SubGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "S4").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void FifthSubGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "SubGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "S5").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void SixthSubGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "SubGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "S6").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }

    [Test]
    public void FirstBasketGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "BasketGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "B1").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }
    [Test]
    public void SecondBasketGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "BasketGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "B2").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }
    [Test]
    public void ThirdBasketGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "BasketGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "B3").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }
    [Test]
    public void FourthBasketGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "BasketGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "B4").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }
    [Test]
    public void FifthBasketGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "BasketGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "B5").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }
    [Test]
    public void SixthBasketGameShouldNotLoadTest()
    {
        AltUnityDriver.LoadScene("Main Menu");
        AltUnityDriver.FindObject(By.NAME, "BasketGameButton").Tap();

        AltUnityDriver.FindObject(By.NAME, "B6").Tap();

        AltUnityDriver.WaitForCurrentSceneToBe("Main Menu");
    }
}