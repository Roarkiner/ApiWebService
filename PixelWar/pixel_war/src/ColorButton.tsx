import { rGBColorToCSSProperty } from "./utils";

const ColorButton: React.FC<{color: RGBColor, setCurrentColor: (color: RGBColor) => void}> = ({ color, setCurrentColor }) => {
    const changeColor = () => {
        setCurrentColor(color)
    }
    
    return(
        <div onClick={changeColor} className="color-button">
            <div style={{backgroundColor: `${rGBColorToCSSProperty(color)}`}}></div>
        </div>
    )
}

export default ColorButton;