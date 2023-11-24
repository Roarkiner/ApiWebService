import { useState } from "react";

const Login: React.FC<{setCurrentUsername: (value: string) => void}> = ({setCurrentUsername}) => {
  const [username, setUsername] = useState("");
  const [registrationError, setRegistrationError] = useState(false);

  const registerUser = () => {
    if (username != "") {
      setRegistrationError(false)
      setCurrentUsername(username)
    } else {
      setRegistrationError(true)
    }
  }
  
  return (
    <>
      <h1>Hello, choose a username : </h1>
      <div>
        <input onChange={(e) => setUsername(e.target.value)} type='text' value={username}></input>
        <button onClick={registerUser}>Access grid</button>
        {(() => {
          if (registrationError) {
            return (
                <div>
                    <span className='error'>Username can't be empty</span>
                </div>
            )
          }
        })()}
      </div>
    </>
  )
}

export default Login;