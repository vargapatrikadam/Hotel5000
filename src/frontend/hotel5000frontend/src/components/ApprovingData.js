import React, {Component} from 'react';
import {Button} from "react-bootstrap";

class ApprovingData extends Component {

    constructor(props) {
        super(props);
        this.state={
            id: null,
            isModifyClicked: false,
            modifiedIdentityNumber: null,
            modifiedTaxNumber: null,
            modifiedRegistrationNumber: null,
            approvingData: []
        }
        this._isMounted = false
    }

    componentDidMount() {
        this._isMounted = true
        this._isMounted && this.getApprovingData(this.props.id)
        this.setState({id: this.props.id})
    }

    componentWillUnmount() {
        this._isMounted = false
    }

    handleIdentityChange = (number) => {
        this.setState({modifiedIdentityNumber: number.target.value});
    }

    handleTaxChange = (number) => {
        this.setState({modifiedTaxNumber: number.target.value});
    }

    handleRegistrationChange = (number) => {
        this.setState({modifiedRegistrationNumber: number.target.value});
    }

    getApprovingData = (id) => {
        fetch("https://localhost:5000/api/users/" + id + "/approvingdata", {
            method: 'GET',
            mode: 'cors',
            headers: {}
        })
            .then(response => response.json())
            .then(responsejson => {
                this._isMounted && this.setState({approvingData: responsejson})
            })
    }

    deleteApprovingData = (userId) => {
        fetch("https://localhost:5000/api/users/" + userId + "/approvingdata", {
            method: 'DELETE',
            mode: "cors",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem('accessToken')
            }
        })
            .then(resp => console.log(resp.status))
    }

    modifyApprovingData = (approvingDataId, identityNumber, taxNumber, registrationNumber) => {

        const data = {
            "id": approvingDataId,
            "identityNumber": identityNumber,
            "taxNumber": taxNumber,
            "registrationNumber": registrationNumber
        }

        fetch("https://localhost:5000/api/users/approvingdata/" + approvingDataId, {
            method: 'PUT',
            mode: "cors",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + localStorage.getItem('accessToken')
            },
            body: JSON.stringify(data)
        })
            .then(resp => resp.status)
            .then(responseStatus => {
                if(responseStatus === 200){
                    this.state.approvingData.map(data => {
                        if(data.id === approvingDataId){
                            data.identityNumber = this.state.modifiedIdentityNumber
                            data.taxNumber = this.state.modifiedTaxNumber
                            data.registrationNumber = this.state.modifiedRegistrationNumber
                        }
                        return data
                    })
                }
            })

    }

    renderApprovingData = () => {
        return this.state.approvingData.map(
            data => {
                return (
                    <div key={data.id}>
                        {data.identityNumber === null ? null : (
                            <div>
                                <label>Identity number:</label>
                                <p>{data.identityNumber}</p>
                            </div>

                        )}

                        {data.taxNumber === null ? null : (
                            <div>
                                <label>Tax number:</label>
                                <p>{data.taxNumber}</p>
                            </div>

                        )}

                        {data.registrationNumber === null ? null : (
                            <div>
                                <label>Registration number:</label>
                                <p>{data.registrationNumber}</p>
                            </div>

                        )}

                        <div hidden={!this.state.isModifyClicked}>
                            <div>
                                <label className="mb-3 mr-2" style={{display: 'inline-block'}}>Identity number:</label>
                                <input type="text" className="mb-3" style={{display: 'inline-block'}} onChange={this.handleIdentityChange}/>
                            </div>
                            <div>
                                <label className="mb-3 mr-2" style={{display: 'inline-block'}}>Tax number:</label>
                                <input type="text" className="mb-3" style={{display: 'inline-block'}} onChange={this.handleTaxChange}/>
                            </div>
                            <div>
                                <label className="mb-3 mr-2" style={{display: 'inline-block'}}>Registration number:</label>
                                <input type="text" className="mb-3" style={{display: 'inline-block'}} onChange={this.handleRegistrationChange}/>
                            </div>
                            <Button className="mb-3" variant="outline-dark" onClick={() => this.modifyApprovingData(data.id, this.state.modifiedIdentityNumber, this.state.modifiedTaxNumber, this.state.modifiedRegistrationNumber)}>Perform</Button>
                        </div>

                        
                        <Button variant="outline-dark" className="mr-3" onClick={() => this.setState({isModifyClicked: true})}>Modify</Button>
                        <Button variant="danger" onClick={() => this.deleteApprovingData(data.id)}>Delete</Button>
                    </div>
                )
            }
        )
    }
    /*TODO:confirmation required on delete button*/

    render() {
        return (
            <div>
                {this.renderApprovingData()}
            </div>
        );
    }
}

export default ApprovingData;