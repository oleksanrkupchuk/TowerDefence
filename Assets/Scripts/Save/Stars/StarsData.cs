[System.Serializable]
public class StarsData {
    public int[] level = new int[10];
    public int stars;

    public StarsData(int countStars, int starsOnCurrentLevel, int levelIndex) {
        stars += countStars;
        level[levelIndex] = starsOnCurrentLevel;
    }

    public StarsData(int countStars) {
        stars = countStars;
    }
}
