import React, {Component} from 'react';
import {Accordion, Button, Card, ListGroup, ListGroupItem} from "react-bootstrap";
import Contacts from "./Contacts";

class User extends Component {

    state={
        users:{}
    }

    componentDidMount() {
        this.getData()
    }


    getData = () => {
        fetch("https://localhost:5000/api/users", {
            method: 'GET',
            mode: 'cors',
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('accessToken')
            }
        })
            .then(response => response.json())
            .then(responsejson => {
                this.setState({users: responsejson})
            })
    }


    renderUsers = () => {
        return Array.from(this.state.users).map(user => {
            return (
                <Card style={{width: '30rem'}} className="mx-auto my-2" key={user.id}>
                    <Card.Body>
                        <Card.Title>{user.username}</Card.Title>
                        <Card.Subtitle>{user.email}</Card.Subtitle>
                    </Card.Body>
                    <ListGroup className="list-group-flush">
                        <ListGroupItem>First name: {user.firstName}</ListGroupItem>
                        <ListGroupItem>Last name: {user.lastName}</ListGroupItem>
                        <ListGroupItem>Role: {user.role}</ListGroupItem>
                        <ListGroupItem>
                            <Accordion>
                                <Card>
                                    <Card.Header>
                                        <Accordion.Toggle as={Button} variant="outline-dark" eventKey={user.id}>
                                            Contacts
                                        </Accordion.Toggle>
                                    </Card.Header>
                                    <Accordion.Collapse eventKey={user.id}>
                                        <Card.Body>
                                            <Contacts id={user.id}/>
                                        </Card.Body>
                                    </Accordion.Collapse>
                                </Card>
                            </Accordion>
                        </ListGroupItem>
                    </ListGroup>
                </Card>
            )
        })
    }


    render() {
        return (
            <div>
                {this.renderUsers()}
            </div>
        );
    }
}

export default User;