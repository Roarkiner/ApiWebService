import './Chat.css'
import { useState } from 'react'

const Chat: React.FC<{currentUsername: string, messages: Array<Message>, addMessage: (message: string) => void}> = ({currentUsername, messages, addMessage}) => {
    const [messageToAdd, setMessageToAdd] = useState("")
    
    const addNewMessage = () => {
        addMessage(messageToAdd)
    }

    return (
        <div className='chat-container'>
            {messages.map((message, index) => {
                console.log(messages)
                return (
                <>
                    <div key={index} className={'message' + (currentUsername == message.username ? " my-message" : "")}>
                        <p><span className='username-header'>{message.username} : </span>{message.message}</p>
                    </div>
                </>
                )
            })}
            
            <div className='send-message'>
                <textarea className='message-input' name="message" id="message" value={messageToAdd} onChange={(e) => setMessageToAdd(e.target.value)} />
                <button className='send-button' onClick={() => addNewMessage()}>Send</button>
            </div>
        </div>
    )
}

export default Chat;