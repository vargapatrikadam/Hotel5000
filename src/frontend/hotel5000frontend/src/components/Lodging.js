import React, {Component} from 'react';
import {Accordion, Button, Card, ListGroup, ListGroupItem} from "react-bootstrap";


class Lodging extends Component {

    constructor(props) {
        super(props);

        this.state = {
            pageNumber: 1,
            resultPerPage: 8,
            data: [],
            rooms: []
        }
    }

    componentDidMount() {
        this.getLodgings()
    }

    getLodgings = () => {
        let url = new URL("https://localhost:5000/api/lodgings"),
            params = {pageNumber: this.state.pageNumber, resultPerPage: this.state.resultPerPage}
            Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))

        fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {

            }
        })
            .then(resp => resp.json())
            .then(responseJson => {
                console.log(responseJson)
                this.setState({data: responseJson})
            })
    }

    renderLodgings = () => {

        const rows = [...Array(Math.ceil(this.state.data.length / 3))]

        const lodgingRows = rows.map((row, index) => this.state.data.slice(index * 3, index * 3 + 3))

        const content = lodgingRows.map((row, index) => {
            return(
                <div className="row" key={index}>
                    {row.map(lodging => {
                        return (
                            <Card style={{width: '25rem'}} key={lodging.id} className="mx-auto my-3">
                                <Card.Body>
                                    <Card.Title>{lodging.name}</Card.Title>
                                    <Card.Subtitle>{lodging.lodgingType}</Card.Subtitle>
                                </Card.Body>
                                <ListGroup className="list-group-flush">
                                    <ListGroupItem>
                                        <Accordion>
                                            <Card>
                                                <Card.Header>
                                                    <Accordion.Toggle as={Button} variant="outline-dark" eventKey={lodging.id}>
                                                        Rooms
                                                    </Accordion.Toggle>
                                                </Card.Header>
                                                <Accordion.Collapse eventKey={lodging.id}>
                                                    <Card.Body>
                                                        {lodging.rooms.map(room => {
                                                            return(
                                                                <Card key={room.id}>
                                                                    <Card.Body>
                                                                        <ListGroup className="list-group-flush">
                                                                            <ListGroupItem>
                                                                                <label>Adult capacity:</label>
                                                                                {room.adultCapacity}
                                                                            </ListGroupItem>
                                                                            <ListGroupItem>
                                                                                <label>Children capacity:</label>
                                                                                {room.childrenCapacity}
                                                                            </ListGroupItem>
                                                                        </ListGroup>


                                                                    </Card.Body>
                                                                </Card>
                                                            )
                                                        })}
                                                    </Card.Body>
                                                </Accordion.Collapse>
                                            </Card>
                                        </Accordion>

                                    </ListGroupItem>
                                </ListGroup>
                            </Card>

                        )
                    })}
                </div>
            )
        })

        return(
            <div>
                {content}
            </div>
        )

        /*return this.state.data.map(lodging => {
            return (
                <Card style={{width: '20rem'}} key={lodging.id}>
                    <Card.Body>
                        <Card.Title>{lodging.name}</Card.Title>
                    </Card.Body>
                </Card>

            )
        })*/
    }


    render() {
        return (
            <div>
                {this.renderLodgings()}
            </div>
        );
    }
}

export default Lodging;