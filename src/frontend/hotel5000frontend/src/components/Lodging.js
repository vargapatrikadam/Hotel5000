import React, {Component} from 'react';
import {Accordion, Button, Card, FormControl, InputGroup, ListGroup, ListGroupItem} from "react-bootstrap";
import {FaSearch} from "react-icons/fa";


class Lodging extends Component {

    constructor(props) {
        super(props);

        this.state = {
            pageNumber: 1,
            resultPerPage: 9,
            data: [],
            nextPageData: [],
            rooms: [],
            fromDateFilter: "",
            toDateFilter: "",
            isSearchClicked: false
        }
    }

    componentDidMount() {
        this.getCurrentLodgings()
        this.getNextData()
    }

    componentDidUpdate(prevProps, prevState) {
        if(prevState.pageNumber !== this.state.pageNumber ||
            prevState.isSearchClicked !== this.state.isSearchClicked){
            this.getCurrentLodgings();
            this.getNextData();
        }
    }

    increasePageNumber = () => {
        if(this.state.nextPageData.length !== 0)
            this.setState({pageNumber: this.state.pageNumber + 1})
    }

    decreasePageNumber = () => {
        if(this.state.pageNumber > 1)
            this.setState({pageNumber: this.state.pageNumber - 1})
    }

    getCurrentLodgings = () => {
        let url = new URL("https://localhost:5000/api/lodgings"),
            params = {pageNumber: this.state.pageNumber, resultPerPage: this.state.resultPerPage,
                      reservableFrom: this.state.fromDateFilter, reservableTo: this.state.toDateFilter}
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

    getNextData = () => {
        let url = new URL("https://localhost:5000/api/lodgings"),
            params = {pageNumber: this.state.pageNumber + 1, resultPerPage: this.state.resultPerPage,
                      reservableFrom: this.state.fromDateFilter, reservableTo: this.state.toDateFilter}
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
                this.setState({nextPageData: responseJson})
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
                            <div className="mx-auto my-3" key={lodging.id}>
                                <Card style={{width: '25rem'}}>
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
                            </div>


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

    handleFromDateChanged = (from) => {
        this.setState({fromDateFilter: from})
        console.log(from)
    }

    handleToDateChanged = (to) => {
        this.setState({toDateFilter: to})
        console.log(to)
    }

    handleSearchClick = () => {
        this.setState({isSearchClicked: !this.state.isSearchClicked, pageNumber: 1})
    }

    render() {
        return (
            <div>
                <div style={{width: '50rem'}} className="mx-auto mt-3">
                    <InputGroup id="input">
                        <FormControl size="lg" placeholder="Country of holiday"></FormControl>

                        <input type="date" as={InputGroup.Append} onChange={(event) => this.handleFromDateChanged(event.target.value)}/>

                        <input type="date" as={InputGroup.Append} onChange={(event) => this.handleToDateChanged(event.target.value)}/>
                        <Button variant="outline-dark" as={InputGroup.Append} onClick={() => this.handleSearchClick()}><FaSearch/>Search</Button>
                    </InputGroup>
                </div>
                {this.renderLodgings()}
                <div className="buttonContainer mx-auto my-3">
                    <Button variant="outline-dark" onClick={() => this.decreasePageNumber()} className="pageButton mr-2" disabled={this.state.pageNumber === 1}>Previous page</Button>
                    <Button variant="outline-dark" onClick={() => this.increasePageNumber()} className="pageButton ml-2" disabled={this.state.nextPageData.length === 0}>Next page</Button>
                </div>
            </div>
        );
    }
}

export default Lodging;