import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";
import { Container, Row, Col } from "react-bootstrap";
import WaitingRoom from "./components/waitingroom";
import { useState } from "react";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import ChatRoom from "./components/ChatRoom";

function App() {
  const [conn, setConnection] = useState();
  const [messages, setMessages] = useState([]);

  const joinChatRoom = async (username, chatroom) => {
    try {
      //initializing conection
      const conn = new HubConnectionBuilder()
        .withUrl("http://localhost:5080/chat")
        .configureLogging(LogLevel.Information)
        .build();

      // set handler
      conn.on("JoinSpecificChatRoom", (username, msg) => {
        setMessages((messages) => [...messages, { username, msg }]);
      });

      conn.on("ReceiveSpecificMessage", (username, msg) => {
        setMessages((messages) => [...messages, { username, msg }]);
      });

      await conn.start();
      await conn.invoke("JoinSpecificChatRoom", { username, chatroom });

      setConnection(conn);
    } catch (e) {
      console.log(e);
    }
  };

  const sendMessage = async (message) => {
    try {
      await conn.invoke("SendMessage", message);
    } catch (e) {
      console.log(e);
    }
  };

  return (
    <div>
      <main>
        <Container>
          <Row class="px-5 my-5">
            <Col sm="12">
              <h1 className="font-wieght-light">Welcome to the F1 ChatApp</h1>
            </Col>
          </Row>
          {!conn ? (
            <WaitingRoom joinChatRoom={joinChatRoom} />
          ) : (
            <ChatRoom messages={messages} sendMessage={sendMessage}></ChatRoom>
          )}
        </Container>
      </main>
    </div>
  );
}

export default App;
