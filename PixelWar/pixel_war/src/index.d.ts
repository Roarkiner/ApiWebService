interface PixelMessage {
    action: string,
    data: any
}

interface PixelActionData {
    id: string,
    x: number,
    y: number,
    color: RGBColor
}

interface RGBColor {
    r: number,
    g: number,
    b: number
}

interface Message {
    username: string,
    message: string
} 