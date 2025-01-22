using UnityEngine;

public static class ColorExtention {

    public static float GrayScale(this Color color) {

        return 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
    }

    public static Color ToGray(this Color color) {
        float grayScale = color.GrayScale();
        return new(grayScale, grayScale, grayScale);
    }
}