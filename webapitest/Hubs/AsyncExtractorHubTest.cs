using webapi.Hubs;
using System.Text;

namespace webapitest.Hubs
{
    public class AsyncExtractorHubTest
    {
        [Theory]
        [InlineData("SampleData", "U2FtcGxlRGF0YQ==")]
        [InlineData("123456789", "MTIzNDU2Nzg5")]
        [InlineData("!@#$%^&*()_+-={}[];\',/.:|,.", "IUAjJCVeJiooKV8rLT17fVtdOycsLy46fCwu")]
        [InlineData("", "")]
        //I've commented below paragraphs due to long waiting time, Kindly uncomment if still want to test. Thank you.
        //[InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras bibendum massa non urna euismod lobortis. Vestibulum congue dignissim tortor, at tempus massa commodo eu. Vestibulum fringilla, erat eget suscipit porta, nibh enim vestibulum odio, eu pellentesque dui purus ut turpis. Integer pulvinar vulputate aliquam. Donec euismod augue quis ligula rutrum, sit amet volutpat purus faucibus. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nunc vitae dui quam. Cras felis risus, ultrices sed eros id, dapibus ornare justo. Aenean tempor ipsum sed mi dapibus, ut ultrices risus pulvinar. Mauris augue sapien, lacinia ut odio nec, elementum finibus lorem. Nulla et odio arcu. Interdum et malesuada fames ac ante ipsum primis in faucibus. Fusce sit amet vehicula ante, ut egestas felis. Aenean nec pellentesque nisl.\r\n\r\nCurabitur ornare mattis mauris pellentesque dignissim. Phasellus vel mi a augue ultricies tincidunt a id metus. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nam eleifend condimentum ligula. Nunc vestibulum malesuada commodo. Aliquam nec risus massa. Aliquam dictum nibh eget commodo fringilla. Aliquam dignissim mauris elit, sit amet vulputate lacus vehicula ut. Fusce imperdiet ornare ex, sed finibus neque molestie in. Suspendisse semper varius venenatis. Proin vehicula blandit efficitur. Quisque hendrerit imperdiet dui.\r\n\r\nNullam eu rutrum neque. Duis sit amet tincidunt dolor. Etiam in libero eu ipsum dictum gravida vel quis turpis. Quisque rutrum sodales nunc, id faucibus ex convallis non. Phasellus ut tortor pretium purus scelerisque tristique eget rhoncus nisi. Donec urna tortor, congue sit amet erat non, feugiat consectetur nisi. Donec at laoreet quam.\r\n\r\nPhasellus eget porttitor tellus, in dictum velit. Vestibulum nisi enim, mollis vehicula quam et, bibendum condimentum ipsum. Duis euismod facilisis congue. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Praesent ante mauris, mattis in tincidunt ac, sodales sed lorem. Donec varius nibh quis pellentesque porttitor. Sed efficitur mauris nibh, vel malesuada ligula dictum vel. Praesent posuere magna id libero tempor, semper rutrum nulla varius. Morbi sodales magna ipsum, ut convallis mi facilisis a. Nullam sed nisl rutrum, congue ante a, eleifend elit. Integer vitae posuere metus. Proin id risus mollis, dapibus nisl et, maximus leo. Quisque non luctus turpis. Maecenas quis urna et ligula tempor porttitor nec quis ante. Suspendisse non efficitur leo, vel convallis nulla.\r\n\r\nSuspendisse feugiat cursus auctor. Cras pretium, purus sed euismod malesuada, dui massa sollicitudin ante, id mollis nisi mauris nec neque. Sed ac quam egestas, sollicitudin tellus vel, sagittis erat. Maecenas et velit lectus. Proin at nibh vel nisi consequat cursus. Sed tincidunt pretium massa, sit amet varius tellus accumsan sit amet. Ut efficitur accumsan nisi eu hendrerit. Mauris condimentum, felis a ornare commodo, est mauris viverra metus, at vehicula velit lacus id eros. Aenean suscipit volutpat velit, non porta tortor aliquam et. Vivamus accumsan purus urna, eu ornare metus porta at. Quisque nisi magna, egestas a ante ut, pulvinar dictum massa. Aliquam pulvinar sagittis sodales. In varius, nisi vel ornare porttitor, sapien enim suscipit felis, in mollis diam dolor vitae lectus. Aliquam eu enim enim. Aenean sed dui at sapien feugiat aliquam eget sit amet erat.", "TG9yZW0gaXBzdW0gZG9sb3Igc2l0IGFtZXQsIGNvbnNlY3RldHVyIGFkaXBpc2NpbmcgZWxpdC4gQ3JhcyBiaWJlbmR1bSBtYXNzYSBub24gdXJuYSBldWlzbW9kIGxvYm9ydGlzLiBWZXN0aWJ1bHVtIGNvbmd1ZSBkaWduaXNzaW0gdG9ydG9yLCBhdCB0ZW1wdXMgbWFzc2EgY29tbW9kbyBldS4gVmVzdGlidWx1bSBmcmluZ2lsbGEsIGVyYXQgZWdldCBzdXNjaXBpdCBwb3J0YSwgbmliaCBlbmltIHZlc3RpYnVsdW0gb2RpbywgZXUgcGVsbGVudGVzcXVlIGR1aSBwdXJ1cyB1dCB0dXJwaXMuIEludGVnZXIgcHVsdmluYXIgdnVscHV0YXRlIGFsaXF1YW0uIERvbmVjIGV1aXNtb2QgYXVndWUgcXVpcyBsaWd1bGEgcnV0cnVtLCBzaXQgYW1ldCB2b2x1dHBhdCBwdXJ1cyBmYXVjaWJ1cy4gQ2xhc3MgYXB0ZW50IHRhY2l0aSBzb2Npb3NxdSBhZCBsaXRvcmEgdG9ycXVlbnQgcGVyIGNvbnViaWEgbm9zdHJhLCBwZXIgaW5jZXB0b3MgaGltZW5hZW9zLiBOdW5jIHZpdGFlIGR1aSBxdWFtLiBDcmFzIGZlbGlzIHJpc3VzLCB1bHRyaWNlcyBzZWQgZXJvcyBpZCwgZGFwaWJ1cyBvcm5hcmUganVzdG8uIEFlbmVhbiB0ZW1wb3IgaXBzdW0gc2VkIG1pIGRhcGlidXMsIHV0IHVsdHJpY2VzIHJpc3VzIHB1bHZpbmFyLiBNYXVyaXMgYXVndWUgc2FwaWVuLCBsYWNpbmlhIHV0IG9kaW8gbmVjLCBlbGVtZW50dW0gZmluaWJ1cyBsb3JlbS4gTnVsbGEgZXQgb2RpbyBhcmN1LiBJbnRlcmR1bSBldCBtYWxlc3VhZGEgZmFtZXMgYWMgYW50ZSBpcHN1bSBwcmltaXMgaW4gZmF1Y2lidXMuIEZ1c2NlIHNpdCBhbWV0IHZlaGljdWxhIGFudGUsIHV0IGVnZXN0YXMgZmVsaXMuIEFlbmVhbiBuZWMgcGVsbGVudGVzcXVlIG5pc2wuDQoNCkN1cmFiaXR1ciBvcm5hcmUgbWF0dGlzIG1hdXJpcyBwZWxsZW50ZXNxdWUgZGlnbmlzc2ltLiBQaGFzZWxsdXMgdmVsIG1pIGEgYXVndWUgdWx0cmljaWVzIHRpbmNpZHVudCBhIGlkIG1ldHVzLiBDbGFzcyBhcHRlbnQgdGFjaXRpIHNvY2lvc3F1IGFkIGxpdG9yYSB0b3JxdWVudCBwZXIgY29udWJpYSBub3N0cmEsIHBlciBpbmNlcHRvcyBoaW1lbmFlb3MuIE5hbSBlbGVpZmVuZCBjb25kaW1lbnR1bSBsaWd1bGEuIE51bmMgdmVzdGlidWx1bSBtYWxlc3VhZGEgY29tbW9kby4gQWxpcXVhbSBuZWMgcmlzdXMgbWFzc2EuIEFsaXF1YW0gZGljdHVtIG5pYmggZWdldCBjb21tb2RvIGZyaW5naWxsYS4gQWxpcXVhbSBkaWduaXNzaW0gbWF1cmlzIGVsaXQsIHNpdCBhbWV0IHZ1bHB1dGF0ZSBsYWN1cyB2ZWhpY3VsYSB1dC4gRnVzY2UgaW1wZXJkaWV0IG9ybmFyZSBleCwgc2VkIGZpbmlidXMgbmVxdWUgbW9sZXN0aWUgaW4uIFN1c3BlbmRpc3NlIHNlbXBlciB2YXJpdXMgdmVuZW5hdGlzLiBQcm9pbiB2ZWhpY3VsYSBibGFuZGl0IGVmZmljaXR1ci4gUXVpc3F1ZSBoZW5kcmVyaXQgaW1wZXJkaWV0IGR1aS4NCg0KTnVsbGFtIGV1IHJ1dHJ1bSBuZXF1ZS4gRHVpcyBzaXQgYW1ldCB0aW5jaWR1bnQgZG9sb3IuIEV0aWFtIGluIGxpYmVybyBldSBpcHN1bSBkaWN0dW0gZ3JhdmlkYSB2ZWwgcXVpcyB0dXJwaXMuIFF1aXNxdWUgcnV0cnVtIHNvZGFsZXMgbnVuYywgaWQgZmF1Y2lidXMgZXggY29udmFsbGlzIG5vbi4gUGhhc2VsbHVzIHV0IHRvcnRvciBwcmV0aXVtIHB1cnVzIHNjZWxlcmlzcXVlIHRyaXN0aXF1ZSBlZ2V0IHJob25jdXMgbmlzaS4gRG9uZWMgdXJuYSB0b3J0b3IsIGNvbmd1ZSBzaXQgYW1ldCBlcmF0IG5vbiwgZmV1Z2lhdCBjb25zZWN0ZXR1ciBuaXNpLiBEb25lYyBhdCBsYW9yZWV0IHF1YW0uDQoNClBoYXNlbGx1cyBlZ2V0IHBvcnR0aXRvciB0ZWxsdXMsIGluIGRpY3R1bSB2ZWxpdC4gVmVzdGlidWx1bSBuaXNpIGVuaW0sIG1vbGxpcyB2ZWhpY3VsYSBxdWFtIGV0LCBiaWJlbmR1bSBjb25kaW1lbnR1bSBpcHN1bS4gRHVpcyBldWlzbW9kIGZhY2lsaXNpcyBjb25ndWUuIFZlc3RpYnVsdW0gYW50ZSBpcHN1bSBwcmltaXMgaW4gZmF1Y2lidXMgb3JjaSBsdWN0dXMgZXQgdWx0cmljZXMgcG9zdWVyZSBjdWJpbGlhIGN1cmFlOyBQcmFlc2VudCBhbnRlIG1hdXJpcywgbWF0dGlzIGluIHRpbmNpZHVudCBhYywgc29kYWxlcyBzZWQgbG9yZW0uIERvbmVjIHZhcml1cyBuaWJoIHF1aXMgcGVsbGVudGVzcXVlIHBvcnR0aXRvci4gU2VkIGVmZmljaXR1ciBtYXVyaXMgbmliaCwgdmVsIG1hbGVzdWFkYSBsaWd1bGEgZGljdHVtIHZlbC4gUHJhZXNlbnQgcG9zdWVyZSBtYWduYSBpZCBsaWJlcm8gdGVtcG9yLCBzZW1wZXIgcnV0cnVtIG51bGxhIHZhcml1cy4gTW9yYmkgc29kYWxlcyBtYWduYSBpcHN1bSwgdXQgY29udmFsbGlzIG1pIGZhY2lsaXNpcyBhLiBOdWxsYW0gc2VkIG5pc2wgcnV0cnVtLCBjb25ndWUgYW50ZSBhLCBlbGVpZmVuZCBlbGl0LiBJbnRlZ2VyIHZpdGFlIHBvc3VlcmUgbWV0dXMuIFByb2luIGlkIHJpc3VzIG1vbGxpcywgZGFwaWJ1cyBuaXNsIGV0LCBtYXhpbXVzIGxlby4gUXVpc3F1ZSBub24gbHVjdHVzIHR1cnBpcy4gTWFlY2VuYXMgcXVpcyB1cm5hIGV0IGxpZ3VsYSB0ZW1wb3IgcG9ydHRpdG9yIG5lYyBxdWlzIGFudGUuIFN1c3BlbmRpc3NlIG5vbiBlZmZpY2l0dXIgbGVvLCB2ZWwgY29udmFsbGlzIG51bGxhLg0KDQpTdXNwZW5kaXNzZSBmZXVnaWF0IGN1cnN1cyBhdWN0b3IuIENyYXMgcHJldGl1bSwgcHVydXMgc2VkIGV1aXNtb2QgbWFsZXN1YWRhLCBkdWkgbWFzc2Egc29sbGljaXR1ZGluIGFudGUsIGlkIG1vbGxpcyBuaXNpIG1hdXJpcyBuZWMgbmVxdWUuIFNlZCBhYyBxdWFtIGVnZXN0YXMsIHNvbGxpY2l0dWRpbiB0ZWxsdXMgdmVsLCBzYWdpdHRpcyBlcmF0LiBNYWVjZW5hcyBldCB2ZWxpdCBsZWN0dXMuIFByb2luIGF0IG5pYmggdmVsIG5pc2kgY29uc2VxdWF0IGN1cnN1cy4gU2VkIHRpbmNpZHVudCBwcmV0aXVtIG1hc3NhLCBzaXQgYW1ldCB2YXJpdXMgdGVsbHVzIGFjY3Vtc2FuIHNpdCBhbWV0LiBVdCBlZmZpY2l0dXIgYWNjdW1zYW4gbmlzaSBldSBoZW5kcmVyaXQuIE1hdXJpcyBjb25kaW1lbnR1bSwgZmVsaXMgYSBvcm5hcmUgY29tbW9kbywgZXN0IG1hdXJpcyB2aXZlcnJhIG1ldHVzLCBhdCB2ZWhpY3VsYSB2ZWxpdCBsYWN1cyBpZCBlcm9zLiBBZW5lYW4gc3VzY2lwaXQgdm9sdXRwYXQgdmVsaXQsIG5vbiBwb3J0YSB0b3J0b3IgYWxpcXVhbSBldC4gVml2YW11cyBhY2N1bXNhbiBwdXJ1cyB1cm5hLCBldSBvcm5hcmUgbWV0dXMgcG9ydGEgYXQuIFF1aXNxdWUgbmlzaSBtYWduYSwgZWdlc3RhcyBhIGFudGUgdXQsIHB1bHZpbmFyIGRpY3R1bSBtYXNzYS4gQWxpcXVhbSBwdWx2aW5hciBzYWdpdHRpcyBzb2RhbGVzLiBJbiB2YXJpdXMsIG5pc2kgdmVsIG9ybmFyZSBwb3J0dGl0b3IsIHNhcGllbiBlbmltIHN1c2NpcGl0IGZlbGlzLCBpbiBtb2xsaXMgZGlhbSBkb2xvciB2aXRhZSBsZWN0dXMuIEFsaXF1YW0gZXUgZW5pbSBlbmltLiBBZW5lYW4gc2VkIGR1aSBhdCBzYXBpZW4gZmV1Z2lhdCBhbGlxdWFtIGVnZXQgc2l0IGFtZXQgZXJhdC4=")]
        public async void SendText_Extractor_ReturnBase64Format(string input, string expected)
        {
            var token = new CancellationTokenSource().Token;
            var hub = new AsyncExtractorHub();
            var output = new StringBuilder();
            var stream = hub.Extract(input, token);

            while (await stream.WaitToReadAsync())
            {
                //Need to wait for each char result from the stream since it gives the character one by one with random delays between 1 to 5 secs
                while (stream.TryRead(out var data))
                {
                    output.Append(data);
                }
            }
            
            Assert.True(output.ToString() == expected);
        }
    }
}