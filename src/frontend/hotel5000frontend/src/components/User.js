import React, {Component} from 'react';
import {Card, ListGroup, ListGroupItem} from "react-bootstrap";

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
                console.log(responsejson)
                this.setState({users: responsejson})
            })
    }

    renderUsers = () => {
        return Array.from(this.state.users).map(user => {
            return (
                <Card style={{width: '18rem'}} className="mx-auto mb-3">
                    <Card.Body>
                        <Card.Title>{user.username}</Card.Title>
                        <Card.Subtitle>{user.email}</Card.Subtitle>
                    </Card.Body>
                    <ListGroup className="list-group-flush">
                        <ListGroupItem>First name: {user.firstName}</ListGroupItem>
                        <ListGroupItem>Last name: {user.lastName}</ListGroupItem>
                        <ListGroupItem>Role: {user.role}</ListGroupItem>
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