import React, {Component, Suspense} from 'react';
import {Accordion, Button, Card, FormControl, ListGroup, ListGroupItem, Modal} from "react-bootstrap";
import Contacts from "./Contacts";
import Spinner from "react-bootstrap/Spinner";
import "./Users.css"
import ApprovingData from "./ApprovingData";
import {refresh} from "./RefreshHelper";



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
            newEmail: "",
            newFirstName: "",
            newLastName: "",
            modalIndex: null,
            isModified: false
        }

        this.handleCloseModal = this.handleCloseModal.bind(this)
        this.handleOpenModal = this.handleOpenModal.bind(this)
    }

    handleOpenModal(e, id){
        this.setState({modalIndex: id});
    }

    handleCloseModal(){
        this.setState({modalIndex: null})
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
            this.getCurrentData();
            this.getNextData();
        }
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

            })
    }

    handleUserNameChanged = (username) => {
        this.setState({newUserName: username.target.value})
    }

    handleEmailChanged = (email) => {
        this.setState({newEmail: email.target.value})
    }

    handleFirstNameChanged = (firstName) => {
        this.setState({newFirstName: firstName.target.value})
    }

    handleLastNameChanged = (lastName) => {
        this.setState({newLastName: lastName.target.value})
    }

    searchById = (id, array) => {
        for (let i=0; i < array.length; i++) {
            if (array[i].id === id) {
                return array[i];
            }
        }
    }

    modifyUser = (userId, username, email, firstName, lastName) => {

        const data = {
            "username": username,
            "email": email,
            "firstName": firstName,
            "lastName": lastName,
        }

        const user = this.searchById(userId, Array.from(this.state.currentPageData))

        if(!data.username)
            data.username = user.username
        if(!data.email)
            data.email = user.email
        if(!data.firstName)
            data.firstName = user.firstName
        if(!data.lastName)
            data.lastName = user.lastName

        fetch("https://localhost:5000/api/users/" + userId, {
            method: 'PUT',
            mode: 'cors',
            headers: {
                "Content-type": "application/json",
                "Authorization": "Bearer " + localStorage.getItem('accessToken')
            },
            body: JSON.stringify(data)
        })
            .then(resp => {
                if(resp.status === 401) {
                    let token = resp.headers.get('token-expired')
                    if(token) {
                        refresh()
                        if(localStorage.getItem('loggedin') === "true") {
                            this.modifyUser(userId, username, email, firstName, lastName)
                            //window.location.reload(true)

                        }
                    }
                }
                else if(resp.status === 200){
                    console.log("token not expired")
                    window.location.reload(false)
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
                            <Button variant="outline-dark" onClick={e => this.handleOpenModal(e, user.id)}>
                                Modify user
                            </Button>
                            <Modal show={this.state.modalIndex === user.id} onHide={() => this.handleCloseModal()}>
                                <Modal.Header closeButton>
                                    <Modal.Title>{user.username}</Modal.Title>
                                </Modal.Header>
                                <Modal.Body>

                                    <div>
                                        <label style={{display: 'inline-block'}} className="mr-3">Username:</label>
                                        <input type="text" style={{display: 'inline-block'}} onChange={this.handleUserNameChanged}
                                               defaultValue={user.username}/>
                                    </div>
                                    <div>
                                        <label style={{display: 'inline-block'}} className="mr-3">E-mail:</label>
                                        <input type="text" style={{display: 'inline-block'}} onChange={this.handleEmailChanged}
                                               defaultValue={user.email}/>
                                    </div>
                                    <div>
                                        <label style={{display: 'inline-block'}} className="mr-3">First name:</label>
                                        <input type="text" style={{display: 'inline-block'}} onChange={this.handleFirstNameChanged}
                                               defaultValue={user.firstName}/>
                                    </div>
                                    <div>
                                        <label style={{display: 'inline-block'}} className="mr-3">Last name:</label>
                                        <input type="text" style={{display: 'inline-block'}} onChange={this.handleLastNameChanged}
                                               defaultValue={user.lastName}/>
                                    </div>
                                </Modal.Body>
                                <Modal.Footer>
                                    <Button variant="secondary" onClick={() => this.handleCloseModal()}>
                                        Close
                                    </Button>
                                    <Button variant="primary" onClick={ () => {this.handleCloseModal(); this.modifyUser(user.id, this.state.newUserName, this.state.newEmail, this.state.newFirstName, this.state.newLastName); this.forceUpdate()}}>
                                        Save Changes
                                    </Button>
                                </Modal.Footer>
                            </Modal>


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