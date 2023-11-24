const canvas = document.getElementById("canvas")
const ctx = canvas.getContext("2d")
const pxlWidth = 10;
const pxlHeight = 10;
const ws = new WebSocket('ws://localhost:8080')
const placedPxlArray = []

canvas.addEventListener('click', event => {
    const rect = canvas.getBoundingClientRect()
    const x = event.clientX - rect.left
    const y = event.clientY - rect.top
    const realX = Math.floor(x/pxlWidth) * pxlWidth
    const realY = Math.floor(y/pxlHeight) * pxlHeight
    const id = `${realX},${realY}`
    const randomRgb = `${getRandomByteNbr()},${getRandomByteNbr()},${getRandomByteNbr()}`
    const data = {action: 'draw', data: {id, x: realX, y: realY, color: `rgb(${randomRgb})`}}

    if(ws.readyState == WebSocket.OPEN) {
        ws.send(JSON.stringify(data))
    }
})

ws.onmessage = event => {
    const {action, data} = JSON.parse(event.data)
    console.log(placedPxlArray)
    console.log(data)
    if(action == 'draw' && placedPxlArray.indexOf(data.id) == -1){
        ctx.fillStyle = data.color
        ctx.fillRect(data.x, data.y, pxlWidth, pxlHeight)
        placedPxlArray.push(data.id)
    }
}

function getRandomByteNbr() {
    return Math.floor(Math.random() * 255);
}