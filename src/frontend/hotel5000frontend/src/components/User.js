import React, {Component, Suspense} from 'react';
import {Accordion, Button, Card, FormControl, ListGroup, ListGroupItem} from "react-bootstrap";
import Contacts from "./Contacts";
import Spinner from "react-bootstrap/Spinner";
import "./Users.css"
import ApprovingData from "./ApprovingData";

class User extends Component {

    constructor(props) {
        super(props);
        this.state={
            previousPageData: {},
            currentPageData: {},
            nextPageData: {},
            pageNumber: 1,
            resultPerPage: 2,
            username: "",
            newUserName: "",
            newPassWord: "",
            newEmail: "",
            newFirstName: "",
            newLastName: "",
            newRole: ""
        }
    }

    setDefaultOFNewData = (userId) => {
        this.state.currentPageData.map(user => {
            if(user.id === userId){
                this.setState({
                    newUserName: user.username,
                    newPassWord: user.password,
                    newEmail: user.email,
                    newFirstName: user.firstName,
                    newLastName: user.lastName,
                    newRole: user.role
                })
            }
            return user
        })
    }

    increasePageNumber = () => {
        if(Array.from(this.state.nextPageData).length !== 0)
            this.setState({pageNumber: this.state.pageNumber + 1})
    }

    decreasePageNumber = () => {
        if(this.state.pageNumber > 1)
            this.setState({pageNumber: this.state.pageNumber - 1})
    }

    componentDidMount() {
        this.getCurrentData();
        this.getNextData();
    }

    componentDidUpdate(prevProps, prevState) {
        if(prevState.pageNumber !== this.state.pageNumber || prevState.username !== this.state.username){
            if(this.state.pageNumber > 1){
                this.getPreviousData();
            }
            this.getCurrentData();
            this.getNextData();
        }
    }

    getPreviousData = () => {
        let url = new URL("https://localhost:5000/api/users"),
            params = {pageNumber:this.state.pageNumber - 1, resultPerPage:this.state.resultPerPage, username:this.state.username}
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
                this.setState({previousPageData: responsejson})
            })
    }
    getCurrentData = () => {
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
                this.setState({currentPageData: responsejson})
            })
    }
    getNextData = () => {
        let url = new URL("https://localhost:5000/api/users"),
            params = {pageNumber:this.state.pageNumber + 1, resultPerPage:this.state.resultPerPage, username:this.state.username}
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
                this.setState({nextPageData: responsejson})
            })
    }

    handleUserNameChanged = (username) => {
        this.setState({newUserName: username.trget.value})
    }
    
    handlePassWordChanged = (password) => {
        this.setState({newPassWord: password.target.value})
    }

    handleEmailChanged = (email) => {
        this.setState({newEmail: email.target.value})
    }

    handleFirstNameChanged = (firstName) => {
        this.setState({newFirstName: firstName})
    }

    handleLastNameChanged = (lastName) => {
        this.setState({newLastName: lastName})
    }

    handleRoleChanged = (role) => {
        this.setState({newRole: role.target.value})
    }

    modifyUser = (userId, username, password, email, firstName, lastName, role) => {

        this.setDefaultOFNewData(userId)

        const data = {
            "id": userId,
            "username": username,
            "password": password,
            "email": email,
            "firstName": firstName,
            "lastName": lastName,
            "role": role
        }

        fetch("https://localhost:5000/api/users/" + userId, {
            method: 'PUT',
            mode: 'cors',
            headers: {
                "Content-type": "application/json",
                "Authorization": "Bearer " + localStorage.getItem('accessToken')
            },
            body: JSON.stringify(data)
        })
            .then(resp => resp.status)
            .then(responseStatus => {
                if(responseStatus === 200){
                    this.state.currentPageData.map(user => {
                        if(user.id === userId){
                            user.username = this.state.newUserName
                            user.password = this.state.newPassWord
                            user.email = this.state.newEmail
                            user.firstName = this.state.newFirstName
                            user.lastName = this.state.newLastName
                            user.role = this.state.newRole
                        }
                        return user
                    })
                }
            })
    }

    renderUsers = () => {
        return Array.from(this.state.currentPageData).map((user, index, array) => {
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
                        <ListGroupItem>
                            <Accordion>
                                <Card>
                                    <Card.Header>
                                        <Accordion.Toggle as={Button} variant="outline-dark" eventKey={user.id}>
                                            Approving data
                                        </Accordion.Toggle>
                                    </Card.Header>
                                    <Accordion.Collapse eventKey={user.id}>
                                        <Card.Body>
                                            <ApprovingData id={user.id}/>
                                        </Card.Body>
                                    </Accordion.Collapse>
                                </Card>
                            </Accordion>
                        </ListGroupItem>
                        <ListGroupItem>
                            <Accordion>
                                <Card>
                                    <Card.Header>
                                        <Accordion.Toggle as={Button} variant="outline-dark" eventKey={user.id}>
                                            Modify user
                                        </Accordion.Toggle>
                                    </Card.Header>
                                    <Accordion.Collapse eventKey={user.id}>
                                        <Card.Body>
                                            <div>
                                                <label style={{display: 'inline-block'}}>Username:</label>
                                                <input type="text" style={{display: 'inline-block'}} onChange={this.handleUserNameChanged}/>
                                            </div>
                                            <div>
                                                <label style={{display: 'inline-block'}}>Password:</label>
                                                <input type="password" style={{display: 'inline-block'}} onChange={this.handlePasswordChanged}/>
                                            </div>
                                            <div>
                                                <label style={{display: 'inline-block'}}>E-mail:</label>
                                                <input type="text" style={{display: 'inline-block'}} onChange={this.handleEmailChanged}/>
                                            </div>
                                            <div>
                                                <label style={{display: 'inline-block'}}>First name:</label>
                                                <input type="text" style={{display: 'inline-block'}} onChange={this.handleFirstNameChanged}/>
                                            </div>
                                            <div>
                                                <label style={{display: 'inline-block'}}>Last name:</label>
                                                <input type="text" style={{display: 'inline-block'}} onChange={this.handleLastNameChanged}/>
                                            </div>
                                            <div>
                                                <label style={{display: 'inline-block'}}>Role:</label>
                                                <input type="text" style={{display: 'inline-block'}} onChange={this.handleRoleChanged}/>
                                            </div>
                                            <Button onClick={this.modifyUser(user.id, user.username, user.password, user.email, user.firstName, user.lastname, user.role)}>Modify</Button>
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
                <FormControl type="text" onChange={(text) => {this.inputChanged(text)}} placeholder="Search by username" className="mx-auto mt-2" style={{width: '30rem'}}/>
                <Suspense fallback={<Spinner />}>
                    {this.renderUsers()}
                </Suspense>
                <div className="buttonContainer mx-auto my-3">
                    <Button variant="outline-dark" onClick={() => this.decreasePageNumber()} className="pageButton mr-2" disabled={this.state.pageNumber === 1}>Previous page</Button>
                    <Button variant="outline-dark" onClick={() => this.increasePageNumber()} className="pageButton ml-2" disabled={Array.from(this.state.nextPageData).length === 0}>Next page</Button>
                </div>

            </div>
        );
    }
}

export default User;