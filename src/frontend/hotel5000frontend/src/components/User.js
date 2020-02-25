import React, {Component, Suspense} from 'react';
import {Accordion, Button, Card, Form, FormControl, ListGroup, ListGroupItem} from "react-bootstrap";
import Contacts from "./Contacts";
import Spinner from "react-bootstrap/Spinner";
import "./Users.css"

class User extends Component {

    constructor(props) {
        super(props);
        this.state={
            users:{},
            pageNumber: 1,
            resultPerPage: 2,
            username: ""
        }
    }


    increasePageNumber = () => {
        this.setState({pageNumber: this.state.pageNumber + 1})
    }

    decreasePageNumber = () => {
        if(this.state.pageNumber > 1)
            this.setState({pageNumber: this.state.pageNumber - 1})
    }

    componentDidMount() {
        this.getData()
    }



    componentDidUpdate(prevProps, prevState) {
        if(prevState.pageNumber !== this.state.pageNumber || prevState.username !== this.state.username)
            this.getData()
    }

    getData = () => {
        let url = new URL("https://localhost:5000/api/users"),
            params = {pageNumber:this.state.pageNumber, resultPerPage:this.state.resultPerPage, username:this.state.username}
        Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

        fetch(url, {
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

    inputChanged = (text) => {
        this.setState({username: text.target.value})
        this.setState({pageNumber: 1})
    }


    render() {
        return (
            <div>
                <FormControl type="text" onChange={(text) => {this.inputChanged(text)}} placeholder="Search by username" className="mx-auto mt-2" style={{width: '30rem'}}></FormControl>
                <Suspense fallback={<Spinner />}>
                    {this.renderUsers()}
                </Suspense>
                <div className="buttonContainer mx-auto my-3">
                    <Button variant="outline-dark" onClick={() => this.decreasePageNumber()} className="pageButton mr-2">Previous page</Button>
                    <Button variant="outline-dark" onClick={() => this.increasePageNumber()} className="pageButton ml-2">Next page</Button>
                </div>

            </div>
        );
    }
}

export default User;