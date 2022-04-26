import * as React from "react";
import {Col, Container, Row} from 'reactstrap';
import {useState} from "react";
import {useHomeStyles, useSocialMediaStyles} from "../home/home.styles";
import {WebApiClient} from "../../common/webApiClient";
import {Button} from "../common/button/Button";
import ReactAudioPlayer from 'react-audio-player';
import * as mm from 'music-metadata-browser';
import {AudioMetadataRequest} from "../../models/AudioMetadataRequest";

export const Audio = () => {
    const [file, setFile] = useState<File>()
    const [submitError, setSubmitError] = useState<string | null>(null);
    const [audioTrack, setAudioTrack] = useState<any>(null);
    const apiClient = WebApiClient();
    const [audioMetadata, setAudioMetadata] = useState<AudioMetadataRequest>();

    function handleChange(event: any) {
        setFile(event.target.files[0])
    }

    const onSubmit = (event: any) => {
        const formData = new FormData();
        // @ts-ignore
        formData.append('audioFile', file)
        apiClient.uploadFilePost('api/audio/audio-file/', formData)
            .then((fileResponse : any) => {
                apiClient.getBlob(`api/audio/stream/${fileResponse.audioFileId}`).then((blobResponse: any) => {
                    const metadata = mm.parseBlob(blobResponse).then((result) => {
                        apiClient.post('api/audio/audio-metadata/', {
                            audioFileId: fileResponse.audioFileId,
                            artist: result.common.artist,
                            title: result.common.title,
                            bitrate: result.format.bitrate,
                            codec: result.format.codec,
                            duration: result.format.duration
                        });
                        var fr = new FileReader;
                        fr.readAsDataURL(blobResponse)
                        fr.onloadend = function () {
                            setAudioTrack(fr.result)
                        }

                    })
                }).catch(e => {
                    setSubmitError(e.status + " " + e.statusText)
                    e.json().then((json: any) => {
                        setSubmitError(e.status + " " + e.statusText + ": " + json);
                    })
                });
            });
    }
    return (
        <Container>
            <Row className="justify-content-md-center align-items-center" style={{height: '90%'}}>
                <Col md="5" className="text-center">
                    <input type="file" onChange={handleChange} name="audioFile" style={{marginBottom: '20px'}}/>
                    <Button onClick={onSubmit}>Upload</Button>
                </Col>
            </Row>
           <Row className="justify-content-md-center align-items-center">
               <Col md="5" className="text-center">
                   <ReactAudioPlayer
                       src={audioTrack}
                       controls
                   />
               </Col>
           </Row>
        </Container>
    );
};