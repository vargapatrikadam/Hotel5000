import React, {Component} from 'react';
import {Accordion, Button, Card, FormLabel, ListGroup, ListGroupItem, Modal} from "react-bootstrap";

class OwnLodging extends Component {

    constructor(props) {
        super(props);

        this.state = {
            lodgings: [],
            modalIndex: null,
            freeIntervals: []
        }

    }

    componentDidMount() {
        this.getOwnLodgings()
    }

    getFreeIntervals = (roomId) => {
        fetch("https://localhost:5000/api/reservations/rooms/" + roomId, {
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

    handleOpenModal(e, id){
        this.setState({modalIndex: id});
    }

    handleCloseModal(){
        this.setState({modalIndex: null})
    }

    getOwnLodgings = () => {
        let url = new URL("https://localhost:5000/api/lodgings"),
            params = {
                owner: localStorage.getItem('username')
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

    formatDate = (dateString) => {
        let options = {year: 'numeric', month: 'numeric', day: 'numeric'}
        return new Date(dateString).toLocaleDateString([], options)
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
                                                                        <h5>Free intervals</h5>
                                                                        {Array.from(this.state.freeIntervals).map((interval, index) => {
                                                                            return(
                                                                                <div key={index}>
                                                                                    <FormLabel>From</FormLabel>
                                                                                    <FormLabel className="mx-2">{this.formatDate(interval.from)}</FormLabel>
                                                                                    <FormLabel>To</FormLabel>
                                                                                    <FormLabel className="mx-2">{this.formatDate(interval.to)}</FormLabel>
                                                                                </div>
                                                                            )
                                                                        })}
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
                                                    </ListGroup>
                                                </div>
                                            )}
                                        )}
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