const express = require("express")
const http = require("http")
const webSocket = require("ws")

const app = express()
const server = http.createServer(app)

const ws = new webSocket.Server({server})

const pxlArray = []
const chatMessages = []

ws.on('connection', socket => {
    socket.on('message', m => {
        console.log(JSON.parse(m))
        const parsedMessage = JSON.parse(m)
        const action = parsedMessage.action

        if (action == 'draw' || action == 'erase') {
            const data = parsedMessage.data
            ws.clients.forEach(client => {
                if(client.readyState == webSocket.OPEN){
                    client.send(JSON.stringify({action, data}))
                }
            })

            const index = pxlArray.findIndex(pxl => pxl.id == data.id)
            if (action == 'draw') {
                if (index != -1)
                    pxlArray[index] = data
                else
                    pxlArray.push(data)
            } else if (action == 'erase') {
                if (index != -1)
                    pxlArray.splice(index, 1)
            }
        } else if (action == "chat") {
            const data = parsedMessage.data
            chatMessages.push(data)
            ws.clients.forEach(client => {
                if(client.readyState == webSocket.OPEN){
                    client.send(JSON.stringify({action: "getAllMessages", data: chatMessages}))
                }
            })

        }
    })

    socket.send(JSON.stringify({action: "getAllPxls", data: pxlArray}))
    socket.send(JSON.stringify({action: "getAllMessages", data: chatMessages}))
})

server.listen(8080, () => {
    console.log('server listening on 8080')
})

