import React, { useState, useEffect } from 'react';
import * as signalR from "@microsoft/signalr"
import { Button, Container, Row, Col, Form } from 'react-bootstrap';

const statusFlag = {
    idle: "idle",
    processing: "processing",
    completed: "completed",
    failed: "failed",
    canceled: "canceled"
};

export default function Extractor() {
    const [hubConnection, setHubConnection] = useState(null);
    const [streamSubscription, setStreamSubscription] = useState(null);
    const [status, setStatus] = useState(statusFlag.idle);
    const [inputTxt, setInputTxt] = useState("");
    const [returnTxt, setReturnTxt] = useState("");
    const [outputTxt, setOutputTxt] = useState("");
        
    useEffect(() => {
        connectToStreamHub();
    }, []);

    useEffect(() => {
        start();
    }, [hubConnection]);

    useEffect(() => {
        setOutputTxt(outputTxt + returnTxt);
    }, [returnTxt]);

    function connectToStreamHub()
    { 
            const connection = new signalR.HubConnectionBuilder()
                .withUrl("https://localhost:7204/asyncExtractorHub")
                .configureLogging(signalR.LogLevel.Information)
            .build();

            setHubConnection(connection);
    };

    async function start() {
        try {
            if (hubConnection === null) {
                connectToStreamHub();
            }
            else { 
                await hubConnection.start();
            }
        } catch (err) {
            console.log(err);
            setTimeout(start(), 5000);
        }
    };

    async function onStartExtract() {
        try {           
            setOutputTxt("");
            setStatus(statusFlag.processing);
            const subscription = hubConnection.stream("Extract", inputTxt.toString())
                .subscribe({
                    next: (item) => {
                        setReturnTxt(item.toString());
                    },
                    complete: () => {
                        setStatus(statusFlag.completed);
                    },
                    error: (err) => {
                        setStatus(statusFlag.failed);
                    },
                });

            setStreamSubscription(subscription);            

        } catch (err) {
            setStatus(statusFlag.failed);
            setTimeout(start, 5000);
        }
    };

    function onCancelConversion() {
        if (streamSubscription) {
            streamSubscription.dispose();
            setStreamSubscription(null);
            setStatus(statusFlag.canceled);
        }
    }

    function onTxtInputChange(event) {
        setInputTxt(event.target.value);        
    }

    function onReset() {
        setStatus(statusFlag.idle);
        setInputTxt("");
        setOutputTxt("");
    }

    return (
        <>
            <div>            
            <ul id="messagesList"></ul>
            </div>

            <Container>
                <Row className="vh-100 d-flex justify-content-md-center align-items-md-center">
                    <Col xs="6" className="p-3 rounded-left border border-info">
                        <Form>
                            <Form.Group className="mb-3" controlId="extractForm.txtInput">
                                <Form.Label>Input a text</Form.Label>
                                <Form.Control rows="3" as="textarea" placeholder="Type any...." required onChange={onTxtInputChange} value={inputTxt} />
                            </Form.Group>

                            {(status == statusFlag.idle || status == statusFlag.canceled) && <div className="d-grid gap-2">
                                <Button variant="primary" onClick={onStartExtract}>
                                    Convert!
                                </Button>
                            </div>}
                        </Form>
                    </Col>
                    <Col xs="6" className="rounded-right bg-info p-3">
                        <Form>
                            <Form.Group className="mb-3" controlId="extractForm.txtResult">
                                <Form.Label>Result</Form.Label>
                                <Form.Control rows="3" as="textarea" disabled readOnly value={outputTxt} />
                            </Form.Group>

                            <div className="d-grid gap-2">
                                {status == statusFlag.processing && <Button variant="outline-dark" onClick={onCancelConversion}>
                                    Cancel
                                </Button>}

                                {(status == statusFlag.completed || status == statusFlag.failed) && <Button variant="outline-dark" onClick={onReset}>
                                    Convert New
                                </Button>}
                            </div>
                        </Form>
                    </Col>
                </Row>
            </Container>
        </>
    );
}