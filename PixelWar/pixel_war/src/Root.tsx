import { useState } from "react";
import Login from "./Login";
import App from "./App";

const Root = () => {
    const [currentUsername, setCurrentUsername] = useState("")

    if(currentUsername == "")
        return(<Login setCurrentUsername={setCurrentUsername}></Login>)
    else
        return(<App currentUsername={currentUsername}></App>)
}

export default Root;