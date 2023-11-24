export const rGBColorToCSSProperty = (rgbColor: RGBColor): string => {
    return `rgb(${rgbColor.r},${rgbColor.g},${rgbColor.b})`;
}