using AidnBackend;
using Xunit;

public class ScoreServiceTests
{

    [Fact]
    public void Add_ShouldCalculateScore()
    {
        // Arrange
        var scoreService = new ScoreService();

        // Act
        int result = scoreService.CalculateScore(39, 43, 19);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Add_ShouldResolveTempScore()
    {
        // Arrange
        var scoreService = new ScoreService();

        // Act
        int result = scoreService.ResolveTempScore(41);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Add_ShouldResolveWrongTempScore()
    {
        // Arrange
        var scoreService = new ScoreService();

        // Act
        int result = scoreService.ResolveTempScore(41);

        // Assert
        Assert.Throws<Exception>(() => scoreService.ResolveTempScore(0));

    }


}
