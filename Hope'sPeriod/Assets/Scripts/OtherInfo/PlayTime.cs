using UnityEngine;

public static class PlayTime {
    private static float playTime;
    private static float startTime;

    public static void Load(float playTime) {
        PlayTime.playTime = playTime;
        startTime = Time.time;
    }

    public static float Save() => playTime + startTime - Time.time;
}

public static class ChapterInfo {
    public static int Chapter { get; private set; } = 0;

    public static void Set(int chapter) => Chapter = chapter;
    public static void NextChapter() => Chapter++;
}