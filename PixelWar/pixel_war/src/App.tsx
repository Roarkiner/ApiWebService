import { useEffect, useState } from 'react'
import useWebSocket from 'react-use-websocket'
import './App.css'
import { rGBColorToCSSProperty } from './utils';
import ColorButton from './ColorButton';
import deleteIcon from './assets/delete.png'
import Chat from './Chat';

const App: React.FC<{currentUsername: string}> = ({currentUsername}) => {
  const ws_url = "ws://localhost:8080"
  const [currentColor, setCurrentColor] = useState({r: 0, g: 0, b: 0});
  const [mode, setMode] = useState("draw");
  const [messages, setMessages] = useState([] as Array<Message>);
  const pxlPerRow = 50;
  const pxlPerCol = 50;
  const pxlWidth = 10;
  const pxlHeight = 10;
  const colorArray: Array<RGBColor> = [
    {r: 255, g: 0, b: 0}, 
    {r: 0, g: 255, b: 0}, 
    {r: 0, g: 0, b: 255}, 
    {r: 255, g: 255, b: 255},
    {r: 0, g: 0, b: 0},
    {r: 255, g: 255, b: 0},
    {r: 255, g: 0, b: 255},
    {r: 0, g: 255, b: 255},
  ];
  let canvas: HTMLCanvasElement;
  let ctx: CanvasRenderingContext2D | null;
  
  useEffect(() => {
    canvas = document.getElementById("canvas") as HTMLCanvasElement;
    ctx = canvas.getContext("2d") 
  })

  const changeCurrentColor = (color: RGBColor) => {
    setCurrentColor(color)
    setMode('draw')
  }

  const {sendJsonMessage} = useWebSocket(ws_url, {
    onMessage: (message) => {
      const parsedMessage: PixelMessage = JSON.parse(message.data)
      const data = parsedMessage.data
      
      if (parsedMessage.action == 'draw') {
        drawPxl(data)
      } else if (parsedMessage.action == 'erase') {
        const eraseData = data
        eraseData.color = {r: 255, g: 255, b: 255}
        drawPxl(eraseData)
      } else if (parsedMessage.action == 'getAllPxls') {
        const pxls: Array<PixelActionData> = data
        pxls.forEach(pxl => {
          drawPxl(pxl)
        })
      } else if (parsedMessage.action == 'getAllMessages') {
        const allMessages: Array<Message> = data
        setMessages(allMessages)
      }
    }
  })
  
  const sendDrawingData= (event: React.MouseEvent<HTMLCanvasElement>): void => {
    const rect = canvas.getBoundingClientRect()
    const x = event.clientX - rect.left
    const y = event.clientY - rect.top
    const normalizedX = Math.floor(x/pxlWidth) * pxlWidth
    const normalizedY = Math.floor(y/pxlHeight) * pxlHeight
    const id = `${normalizedX},${normalizedY}`
    const data: PixelMessage = {action: mode, data: {id, x: normalizedX, y: normalizedY, color: currentColor}}

    sendJsonMessage(data)
  }

  const drawPxl = (data: PixelActionData): void => {
    if(ctx == null) {
      console.log("Impossible to get the canvas' rendering context")
    } else {
      ctx.fillStyle = rGBColorToCSSProperty(data.color)
      ctx.fillRect(data.x, data.y, pxlWidth, pxlHeight)
    }
  }

  const addMessage = (message: string) => {
    sendJsonMessage({action: 'chat', data: {username: currentUsername, message: message}})
  }

  return (
    <>
    <div className='main-container'>
      <main>
        <h1>Hello, {currentUsername} !</h1>
        <div className='canva-container'>
          <canvas onClick={sendDrawingData} id="canvas" width={pxlWidth * pxlPerRow} height={pxlHeight * pxlPerCol}></canvas>
        </div>
        <div className='color-container'>
          <div className='current-color-container'>
          {(() => {
            if (mode == "draw"){
              return <>
                <span>Current color :</span>
                <div className='current-color' style={{backgroundColor: `${rGBColorToCSSProperty(currentColor)}`}}></div>
              </>
            }
            else if (mode == "erase") {
              return <>
                <span>Erase mode</span>
                <img className='delete-button-img' src={deleteIcon}></img>
              </>
            }
          })()}
          </div>
          <div className='color-picker'>
            {(() => {
              const elements: JSX.Element[] = [];
              let key = 1;

              colorArray.forEach(colorElement => {
                elements.push(
                  <ColorButton key={key} color={colorElement} setCurrentColor={changeCurrentColor}></ColorButton>
                );
                key++;
              })

              return elements;
            })()}
            <img onClick={() => {setMode("erase")}} className='delete-button' src={deleteIcon}></img>
          </div>
        </div>
      </main>
      <aside>
        <Chat messages={messages} addMessage={addMessage} currentUsername={currentUsername}></Chat>
      </aside>
    </div>
    </>
  )
}

export default App
