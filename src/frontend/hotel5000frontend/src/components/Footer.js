import React from "react";
import {FaFacebookSquare, FaPhone, FaEnvelope } from 'react-icons/fa';

var iconstyle = {
    color: "#23d9d2",
    marginLeft: "20px",
    margibRight: "30px",
    height: "30px",
    width: "30px"
}

function Footer() {
    return(
      <div>
          <div style={{marginBottom: 10}}>
              <div>
                  <h3 style={{color: "#23d9d2"}}>Contact us</h3>
              </div>
              <FaFacebookSquare style={iconstyle}/>
              <FaPhone style={iconstyle}/>
              <FaEnvelope style={iconstyle}/>
          </div>
      </div>
    );
}

export default Footer;