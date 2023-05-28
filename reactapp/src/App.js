import React, { useState, useEffect } from 'react';
import * as signalR from "@microsoft/signalr"
import { Button, Container, Row, Col, Form, Spinner, Stack, Alert, Modal } from 'react-bootstrap';

//Enum for updating display
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
    const [alertMsg, setAlertMsg] = useState(null);
    const [showNewConfirm, setShowNewConfirm] = useState(false);

    //First Load of the component
    useEffect(() => {
        connectToStreamHub();
    }, []);

    //Once the building of hub connections is setup
    useEffect(() => {
        start();
        // eslint-disable-next-line
    }, [hubConnection]);

    //This process the actual output to display coming from the hub stream
    useEffect(() => {
        setOutputTxt(outputTxt + returnTxt);
        // eslint-disable-next-line
    }, [returnTxt]);

    
    useEffect(() => {
        let alert = {
            variant: "success",
            headerMsg: "Done",
            bodyMsg: "Your text has been converted successfully."
        };

        if (status === statusFlag.completed) {
            //alert object Already declared above;
        }
        else if (status === statusFlag.failed) {
            alert.variant = "danger";
            alert.headerMsg = "Failed";
            alert.headerMsg = "There was a problem processing your text, kindly try again.";
        }
        else {
            alert = null;
        }

        setAlertMsg(alert);
    }, [status]);

    function connectToStreamHub()
    {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(process.env.REACT_APP_BE_EXTRACTOR_HUB_URI)
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
            setTimeout(start(), 5000);
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
        setShowNewConfirm(false);
    }

    return (
        <>
            <Container>
                <Row className="vh-100 d-flex justify-content-md-center align-items-md-center">
                    <Col xs="12">
                        { alertMsg && (
                        <Alert variant={alertMsg.variant} dismissible>
                                <Alert.Heading>{alertMsg.headerMsg}</Alert.Heading>
                            <p>
                               {alertMsg.bodyMsg}
                            </p>
                        </Alert>
                        )}

                        <Container className="border rounded bg-light">
                            <Row>
                                <Col xs="12" className="p-3 border bg-primary text-white">
                                 Welcome to base64 conversion tool
                                </Col>
                            </Row>
                            <Row>
                                <Col xs="6" className="p-3">
                                    <Form>
                                        <Form.Group className="mb-3" controlId="extractForm.txtInput">
                                            <Form.Label>Input a text</Form.Label>
                                            <Form.Control rows="10" as="textarea" placeholder="Type any...." required onChange={onTxtInputChange} value={inputTxt} />
                                        </Form.Group>

                                        {(status === statusFlag.idle || status === statusFlag.canceled) && <div className="d-grid gap-2">
                                            <Button variant="primary" onClick={onStartExtract}>
                                                Convert!
                                            </Button>
                                        </div>}
                                    </Form>
                                </Col>
                                <Col xs="6" className="p-3">
                                    <Form>
                                        <Form.Group className="mb-3" controlId="extractForm.txtResult">
                                            <Stack direction="horizontal" gap={5}>
                                                <Form.Label>Result</Form.Label>
                                                {status === statusFlag.processing && <Spinner className="ms-auto" variant="secondary" animation="grow" size="sm" />}
                                            </Stack>

                                            <Form.Control rows="10" as="textarea" disabled readOnly value={outputTxt} />
                                        </Form.Group>

                                        <div className="d-grid gap-2">
                                            {status === statusFlag.processing && <Button variant="danger" onClick={onCancelConversion}>
                                                Cancel
                                            </Button>}

                                            {(status === statusFlag.completed || status === statusFlag.failed) && <Button variant="success" onClick={() => setShowNewConfirm(true)}>
                                                Convert New
                                            </Button>}
                                        </div>
                                    </Form>
                                </Col>
                            </Row>
                        </Container>
                    </Col>                    
                </Row>
            </Container>


            <Modal size="lg" centered show={showNewConfirm}>
                <Modal.Header>
                    <Modal.Title>
                        Confirm
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <p>
                        Your previous result and input text will be removed, do you still want to continue?
                    </p>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="primary" onClick={onReset}>Yes</Button>
                    <Button variant="danger" onClick={() => setShowNewConfirm(false)}>Cancel</Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}