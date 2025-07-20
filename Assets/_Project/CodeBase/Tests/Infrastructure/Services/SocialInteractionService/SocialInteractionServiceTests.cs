using CodeBase.Infrastructure.Services.SocialInteractionService;
using NUnit.Framework;

[TestFixture]
public class SocialInteractionServiceTests
{
    private SocialInteractionService _service;

    [SetUp]
    public void Setup()
    {
        _service = new SocialInteractionService(initialCharisma: 30);
    }

    // Test: First interaction with new player should initialize friendship
    [Test]
    public void FirstInteraction_InitializesFriendship()
    {
        var result = _service.Interact("new_player", InteractionType.Chat);
        Assert.AreEqual(1, result.NewFriendshipLevel);
    }

    // Test: Should not allow more than 3 gifts per day
    [Test]
    public void GiftLimit_EnforcesDailyMaximum()
    {
        // Send 3 gifts (limit)
        _service.Interact("p1", InteractionType.Gift);
        _service.Interact("p2", InteractionType.Gift);
        _service.Interact("p3", InteractionType.Gift);
        
        // Fourth gift should fail
        var result = _service.Interact("p4", InteractionType.Gift);
        Assert.AreEqual(1, result.NewFriendshipLevel); // No level increase
    }

    // Test: Joint activities should unlock content at specific levels
    [Test]
    public void Activities_UnlockContentAtThresholds()
    {
        // Perform 4 joint activities (4*2 = 8 points -> level 5)
        for (int i = 0; i < 4; i++)
        {
            _service.Interact("friend", InteractionType.JointActivity);
        }

        var result = _service.Interact("friend", InteractionType.Chat);
        Assert.AreEqual("ParkDance", result.UnlockedMiniGame);
    }

    // Test: Charisma stat should affect chat points
    [Test]
    public void Charisma_ModifiesChatEffectiveness()
    {
        var lowCharismaService = new SocialInteractionService(10);
        var highCharismaService = new SocialInteractionService(50);
        
        var lowResult = lowCharismaService.Interact("p1", InteractionType.Chat);
        var highResult = highCharismaService.Interact("p1", InteractionType.Chat);
        
        // High charisma: 1 + (50/20)=3 → level 4
        // Low charisma: 1 + (10/20)=1 → level 2
        Assert.AreEqual(4, highResult.NewFriendshipLevel);
        Assert.AreEqual(2, lowResult.NewFriendshipLevel);
    }

    // Test: Daily reset should clear gift counter
    [Test]
    public void DailyReset_ClearsGiftLimit()
    {
        _service.Interact("p1", InteractionType.Gift);
        _service.Interact("p2", InteractionType.Gift);
        _service.ResetDailyLimits();
        
        var result = _service.Interact("p3", InteractionType.Gift);
        Assert.AreEqual(4, result.NewFriendshipLevel); // Success after reset
    }
}