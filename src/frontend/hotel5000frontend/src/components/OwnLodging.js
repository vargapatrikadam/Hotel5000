import React, {Component} from 'react';
import {Accordion, Button, Card, Form, FormLabel, ListGroup, ListGroupItem, Modal} from "react-bootstrap";
import {refresh} from "./RefreshHelper";
import {BaseUrl} from './FetchHelper'


class OwnLodging extends Component {

    constructor(props) {
        super(props);

        this.setSelectedCurrency = this.setSelectedCurrency.bind(this)
        this.setSelectedCountry = this.setSelectedCountry.bind(this)

        this.state = {
            lodgings: [],
            modalIndex: null,
            freeIntervals: [],

            freeFrom: "",
            freeTo: "",

            currency: 'Forint',
            adults: null,
            children: null,
            price: null,

            countryCode: "HU",
            county: "",
            city: "",
            postalCode : "",
            street: "",
            houseNumber: "",
            floor: "",
            doorNumber: ""
        }

    }

    componentDidMount() {
        this.getOwnLodgings()
    }

    getFreeIntervals = (roomId) => {
        fetch(BaseUrl + "api/reservations/rooms/" + roomId, {
            method: 'GET',
            mode: "cors",
            headers: {

            }
        })
            .then(response => response.json())
            .then(responseJson => {
                this.setState({freeIntervals: responseJson})
            })
    }
    getOwnLodgings = () => {
        let url = new URL(BaseUrl + "api/lodgings"),
            params = {
                owner: sessionStorage.getItem('username')
            }
            Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

        fetch(url, {
            method: 'GET',
            mode: "cors",
            headers: {

            }
        })
            .then(resp => resp.json())
            .then(responseJson => {
                this.setState({lodgings: responseJson})
            })
    }
    addReservationWindow = (id, from, to) => {
        const data = {
            lodgingId: id,
            From: from,
            To: to
        }

        fetch(BaseUrl + "api/lodgings/reservationwindows", {
            method: 'POST',
            mode: "cors",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('accessToken')
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if(response.status === 200){
                    alert("Reservation window successfully added to lodging")
                    window.location.reload(false)
                }
                else if(response.status === 401){
                    let token = response.headers.get('token-expired')
                    if(token) {
                        refresh().then(() => {
                            this.addReservationWindow(id, from, to)
                        }).catch((error) => {
                            console.log(error)
                        })
                    }
                }
            })
    }
    deleteLodging = (id) => {
        fetch(BaseUrl + "api/lodgings/lodgings/" + id, {
            method: 'DELETE',
            mode: "cors",
            headers: {
                "Authorization": "Bearer " + sessionStorage.getItem('accessToken')
            }
        })
            .then(response => {
                if (response.status === 401) {
                    let token = response.headers.get('token-expired');
                    if(token) {
                        refresh().then(() => {
                            this.deleteLodging(id)
                        })
                            .catch((error) => {
                                console.log(error)
                            })
                    }
                }
                else if(response.status === 200){
                    alert("Lodging successfully deleted")
                    window.location.reload(false)
                }
            })
    }
    deleteRoom = (id) => {
        fetch(BaseUrl + "api/lodgings/rooms/" + id, {
            method: 'DELETE',
            mode: "cors",
            headers: {
                "Authorization": "Bearer " + sessionStorage.getItem('accessToken')
            }
        })
            .then(response => {
                if (response.status === 401) {
                    let token = response.headers.get('token-expired');
                    if(token) {
                        refresh().then(() => {
                            this.deleteRoom(id)
                        })
                            .catch((error) => {
                                console.log(error)
                            })
                    }
                }
                else if(response.status === 200){
                    alert("Room successfully deleted")
                    window.location.reload(false)
                }
            })
    }
    modifyRoom = (id) => {
        const data = {
            adultCapacity: this.state.adults,
            childrenCapacity: this.state.children,
            price: this.state.price,
            currency: this.state.currency
        }

        fetch(BaseUrl + "api/lodgings/rooms/" + id, {
            method: 'PUT',
            mode: "cors",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('accessToken')
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if (response.status === 401) {
                    let token = response.headers.get('token-expired');
                    if(token) {
                        refresh().then(() => {
                            this.modifyRoom(id)
                        })
                            .catch((error) => {
                                console.log(error)
                            })
                    }
                }
                else if(response.status === 200){
                    alert("Room successfully modified")
                    window.location.reload(false)
                }
            })
    }
    modifyAddress = (id) => {
        const data = {
            countryCode: this.state.countryCode,
            county: this.state.county,
            city: this.state.city,
            postalCode: this.state.postalCode,
            street: this.state.street,
            houseNumber: this.state.houseNumber,
            floor: this.state.floor, //optional
            doorNumber: this.state.doorNumber //optional
        }
        fetch(BaseUrl + "api/lodgings/addresses/" + id, {
            method: 'PUT',
            mode: "cors",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('accessToken')
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if (response.status === 401) {
                    let token = response.headers.get('token-expired');
                    if(token) {
                        refresh().then(() => {
                            this.modifyAddress(id)
                        })
                            .catch((error) => {
                                console.log(error)
                            })
                    }
                }
                else if(response.status === 200){
                    alert("Address successfully modified")
                    window.location.reload(false)
                }
            })
    }

    formatDate = (dateString) => {
        let options = {year: 'numeric', month: 'numeric', day: 'numeric'}
        return new Date(dateString).toLocaleDateString([], options)
    }
    fillStateForRoom = (adults, children, price) => {
        this.setState({adults: adults, children: children, price: price})
    }
    fillStateForAddress = (country, county, city, postal, street, hNumber, floor, dNumber) => {
        this.setState({countryCode: country, county: county, city: city, postalCode: postal, street: street, houseNumber: hNumber, floor: floor, doorNumber: dNumber})
    }

    handleOpenModal(e, id){
        this.setState({modalIndex: id});
    }
    handleCloseModal(){
        this.setState({modalIndex: null})
    }
    handleFromChanged = (from) => {
        this.setState({freeFrom: from})
    }
    handleToChanged = (to) => {
        this.setState({freeTo: to})
    }

    setSelectedCurrency(event) {
        this.setState({currency: event.target.value})
    }
    handleAdultCapacityChanged = event =>{
        this.setState({adults: parseInt(event.target.value)})
    }
    handleChildrenCapacityChanged = event => {
        this.setState({children: parseInt(event.target.value)})
    }
    handlePriceChanged = event => {
        this.setState({price: parseInt(event.target.value)})
    }

    setSelectedCountry(event){
        this.setState({countryCode: event.target.value})
    }
    handleCountyChanged = (county) => {
        this.setState({county: county.target.value})
    }
    handleCityChanged = (city) => {
        this.setState({city: city.target.value})
    }
    handlePostalCodeChanged = event => {
        this.setState({postalCode: parseInt(event.target.value)})
    }
    handleStreetChanged = (street) => {
        this.setState({street: street.target.value})
    }
    handleHouseNumberChanged = (hNumber) => {
        this.setState({houseNumber: hNumber.target.value})
    }
    handleFloorChanged = (floor) => {
        this.setState({floor: floor.target.value})
    }
    handleDoorNumberChanged = (dNumber) => {
        this.setState({doorNumber: dNumber.target.value})
    }

    renderOwnLodgings = () => {
        return(
            this.state.lodgings.map(lodging => {
                return(
                    <div key={lodging.id}>
                        <Card style={{width: '25rem'}} className="my-3 mx-auto">
                            <Card.Body>
                                <Card.Title>{lodging.name}</Card.Title>
                                <Card.Subtitle>{lodging.lodgingType}</Card.Subtitle>
                            </Card.Body>
                            <ListGroup className="list-group-flush">
                                <Button variant="outline-dark" onClick={e => this.handleOpenModal(e, lodging.id)}>
                                    Details
                                </Button>
                                <Modal show={this.state.modalIndex === lodging.id} onHide={() => this.handleCloseModal()}>
                                    <Modal.Header closeButton>
                                        <Modal.Title>{lodging.name}</Modal.Title>
                                    </Modal.Header>
                                    <Modal.Body>
                                        <h5 className="mt-3">Rooms</h5>
                                        {lodging.rooms.map( room => {
                                            return(
                                                <div key={room.id}>
                                                    <Accordion>
                                                        <Card>
                                                            <Card.Header>
                                                                <Accordion.Toggle as={Button} variant="outline-dark"
                                                                                  eventKey={room.id} onClick={() => this.getFreeIntervals(room.id)}>
                                                                    {room.adultCapacity + room.childrenCapacity} capacity room
                                                                </Accordion.Toggle>
                                                            </Card.Header>
                                                            <Accordion.Collapse eventKey={room.id}>
                                                                <ListGroup>
                                                                    <ListGroupItem>
                                                                        Adult capacity: {room.adultCapacity}
                                                                    </ListGroupItem>
                                                                    <ListGroupItem>
                                                                        Children capacity: {room.childrenCapacity}
                                                                    </ListGroupItem>
                                                                    <ListGroupItem>
                                                                        Price: {room.price} {room.currency}
                                                                    </ListGroupItem>
                                                                    <ListGroupItem>
                                                                        <Accordion>
                                                                            <Accordion.Toggle as={Button} variant="outline-secondary"
                                                                                                eventKey={room.id} onClick={() => this.fillStateForRoom(room.adultCapacity, room.childrenCapacity, room.price)}>
                                                                                Modify room
                                                                            </Accordion.Toggle>
                                                                            <Accordion.Collapse eventKey={room.id}>
                                                                                <ListGroup>
                                                                                    <ListGroupItem>
                                                                                        <Form.Label>Adults</Form.Label>
                                                                                        <Form.Control type="number" placeholder="Adult capacity" min="0" onChange={this.handleAdultCapacityChanged}/>
                                                                                        <Form.Label>Children</Form.Label>
                                                                                        <Form.Control type="number" placeholder="Children capacity" min="0" onChange={this.handleChildrenCapacityChanged}/>
                                                                                        <Form.Label>Price</Form.Label>
                                                                                        <Form.Control placeholder="Price of room" step="50" min="0" type="number" onChange={this.handlePriceChanged}/>
                                                                                        <Form.Label>Currency</Form.Label>
                                                                                        <Form.Control as="select" onChange={this.setSelectedCurrency}>
                                                                                            <option value="Forint">Forint</option>
                                                                                            <option value="Euro">Euro</option>
                                                                                        </Form.Control>

                                                                                    </ListGroupItem>
                                                                                    <ListGroupItem>
                                                                                        <Button variant="outline-success" onClick={() => this.modifyRoom(room.id)}>
                                                                                            Update
                                                                                        </Button>
                                                                                    </ListGroupItem>
                                                                                </ListGroup>
                                                                            </Accordion.Collapse>
                                                                        </Accordion>
                                                                    </ListGroupItem>
                                                                    <ListGroupItem>
                                                                        <Button variant="outline-danger"
                                                                                onClick={() => {if(window.confirm("Are you sure you want to delete this room?")) this.deleteRoom(room.id)}}>
                                                                            Delete room
                                                                        </Button>
                                                                    </ListGroupItem>
                                                                </ListGroup>
                                                            </Accordion.Collapse>
                                                        </Card>
                                                    </Accordion>
                                                </div>
                                            )}
                                        )}
                                        <h5 className="mt-3">Address</h5>
                                        {lodging.lodgingAddresses.map( address => {
                                            return(
                                                <div key={address.id}>
                                                    <ListGroup>
                                                        <ListGroupItem>
                                                            {address.countryCode}, {address.country}, {address.county}, {address.postalCode},
                                                            {address.city}, {address.street}, {address.houseNumber}
                                                        </ListGroupItem>
                                                        <ListGroupItem hidden={address.floor === null}>
                                                            Floor: {address.floor}
                                                        </ListGroupItem>
                                                        <ListGroupItem hidden={address.doorNumber === null}>
                                                            Door number: {address.doorNumber}
                                                        </ListGroupItem>
                                                        <ListGroupItem>
                                                            <Accordion>
                                                                <Accordion.Toggle as={Button} variant="outline-secondary"
                                                                                    eventKey={address.id} onClick={() => this.fillStateForAddress(address.country, address.county, address.city,
                                                                                                                                                  address.postalCode, address.street, address.houseNumber,
                                                                                                                                                  address.floor, address.doorNumber)}>
                                                                    Modify address
                                                                </Accordion.Toggle>
                                                                <Accordion.Collapse eventKey={address.id}>
                                                                    <ListGroup>
                                                                        <Form.Label>Country</Form.Label>
                                                                        <Form.Control as="select" onChange={this.setSelectedCountry}>
                                                                            <option value="HU">Hungary</option>
                                                                            <option value="SK">Slovakia</option>
                                                                            <option value="HR">Croatia</option>
                                                                            <option value="RS">Serbia</option>
                                                                            <option value="IT">Italy</option>
                                                                        </Form.Control>
                                                                        <Form.Label>County</Form.Label>
                                                                        <Form.Control type="text" placeholder="County" onChange={(county) => this.handleCountyChanged(county)}/>
                                                                        <Form.Label>City</Form.Label>
                                                                        <Form.Control type="text" placeholder="City" onChange={(city) => this.handleCityChanged(city)}/>
                                                                        <Form.Label>Postal code</Form.Label>
                                                                        <Form.Control type="number" placeholder="Postal code" min="0" onChange={this.handlePostalCodeChanged}/>
                                                                        <Form.Label>Street</Form.Label>
                                                                        <Form.Control type="text" placeholder="Street" onChange={(street) => this.handleStreetChanged(street)}/>
                                                                        <Form.Label>House number</Form.Label>
                                                                        <Form.Control type="text" placeholder="House number" onChange={(hNumber) => this.handleHouseNumberChanged(hNumber)}/>
                                                                        <Form.Label>Floor</Form.Label>
                                                                        <Form.Control type="text" placeholder="Floor (optional)" onChange={(floor) => this.handleFloorChanged(floor)}/>
                                                                        <Form.Label>Door number</Form.Label>
                                                                        <Form.Control type="text" placeholder="Door number (optional)" onChange={(dNumber) => this.handleDoorNumberChanged(dNumber)}/>
                                                                        <ListGroupItem>
                                                                            <Button variant="outline-success" onClick={() => this.modifyAddress(address.id)}>
                                                                                Update
                                                                            </Button>
                                                                        </ListGroupItem>
                                                                    </ListGroup>
                                                                </Accordion.Collapse>
                                                            </Accordion>
                                                        </ListGroupItem>
                                                    </ListGroup>
                                                </div>
                                            )}
                                        )}
                                        <h5 className="mt-3">Reservation windows</h5>
                                        {lodging.reservationWindows.map( window => {
                                            return (
                                                <div key={window.id} className="mx-auto" style={{textAlign: 'center'}}>
                                                    <p>From: {this.formatDate(window.from)} &emsp; To: {this.formatDate(window.to)}</p>
                                                </div>
                                            )
                                        })}
                                        <Accordion>
                                            <Accordion.Toggle as={Button} variant="outline-dark"
                                                              eventKey={lodging.id}>
                                                Add reservation window
                                            </Accordion.Toggle>
                                            <Accordion.Collapse eventKey={lodging.id}>
                                                <ListGroup>
                                                    <ListGroupItem>
                                                        <FormLabel>Free from:</FormLabel>
                                                        <input type="date" onChange={(event) => this.handleFromChanged(event.target.value)}/><br/>
                                                        <FormLabel>To:</FormLabel>
                                                        <input type="date" className="ml-5" onChange={(event) => this.handleToChanged(event.target.value)}/>
                                                        <Button variant="outline-dark" onClick={() => this.addReservationWindow(lodging.id, this.state.freeFrom, this.state.freeTo)}>Add reservationWindow to lodging</Button>
                                                    </ListGroupItem>
                                                </ListGroup>
                                            </Accordion.Collapse>
                                        </Accordion>
                                        <Button variant="outline-danger"
                                                onClick={() => {if(window.confirm('Are you sure you want to delete this lodging?'))
                                                                this.deleteLodging(lodging.id)}}>
                                            Delete lodging
                                        </Button>
                                    </Modal.Body>
                                </Modal>
                            </ListGroup>
                        </Card>
                    </div>
                )
            })
        )

    }

    render() {
        return (
            <div>
                {this.renderOwnLodgings()}
            </div>
        );
    }
}

export default OwnLodging;