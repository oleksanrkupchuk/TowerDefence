[System.Serializable]
public class StarsData {
    public int[] level = new int[10];
    public int stars;

    public StarsData(int countStars, int levelIndex) {
        if (level[levelIndex] < countStars) {
            stars += countStars - level[levelIndex];
            level[levelIndex] = countStars;
        }
    }
}
