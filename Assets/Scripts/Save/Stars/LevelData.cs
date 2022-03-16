using System.Collections.Generic;

[System.Serializable]
public class LevelData {
    public List<Level> levels;
    public int stars;

    public LevelData(int countStars, List<Level> levels) {
        stars = countStars;
        this.levels = levels;
    }
}

[System.Serializable]
public class Level {
    public int levelIndex;
    public int stars;
    public bool isUnlock;

    public Level(int levelIndex, int stars, bool isUnlock) {
        this.stars = stars;
        this.levelIndex = levelIndex;
        this.isUnlock = isUnlock;
    }
}
