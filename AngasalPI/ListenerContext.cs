using System;
using System.IO;
using System.Net;
using System.Text;

/// <summary>
/// Angasal programming interface namespace
/// </summary>
namespace AngasalPI
{
    /// <summary>
    /// Listener context class
    /// </summary>
    public class ListenerContext
    {
        /// <summary>
        /// HTTP listener context
        /// </summary>
        private HttpListenerContext httpListenerContext;

        /// <summary>
        /// Response bytes
        /// </summary>
        public byte[] responseBytes;

        /// <summary>
        /// Response
        /// </summary>
        public string response;

        /// <summary>
        /// Standard output
        /// </summary>
        private TextWriter standardOutput;

        /// <summary>
        /// Error output
        /// </summary>
        private TextWriter errorOutput;

        /// <summary>
        /// HTTP listener context
        /// </summary>
        public HttpListenerContext HTTPListenerContext => httpListenerContext;

        /// <summary>
        /// Response
        /// </summary>
        public byte[] ResponseBytes
        {
            get
            {
                if (responseBytes == null)
                {
                    responseBytes = new byte[0];
                }
                return responseBytes;
            }
            set
            {
                if (value != null)
                {
                    response = null;
                    responseBytes = value;
                }
            }
        }

        /// <summary>
        /// Response
        /// </summary>
        public string Response
        {
            get
            {
                try
                {
                    if (response == null)
                    {
                        response = ResponseEncoding.GetString(responseBytes);
                    }
                }
                catch (Exception e)
                {
                    ErrorOutput.WriteLine(e);
                }
                if (response == null)
                {
                    response = "";
                }
                return response;
            }
            set
            {
                if (value != null)
                {
                    try
                    {
                        responseBytes = ResponseEncoding.GetBytes(value);
                        response = value;
                    }
                    catch (Exception e)
                    {
                        ErrorOutput.WriteLine(e);
                    }
                }
            }
        }

        /// <summary>
        /// Request encoding
        /// </summary>
        public Encoding RequestEncoding
        {
            get
            {
                return httpListenerContext.Request.ContentEncoding;
            }
        }

        /// <summary>
        /// Response encoding
        /// </summary>
        public Encoding ResponseEncoding
        {
            get
            {
                if (httpListenerContext.Response.ContentEncoding == null)
                {
                    httpListenerContext.Response.ContentEncoding = Encoding.UTF8;
                }
                return httpListenerContext.Response.ContentEncoding;
            }
            set
            {
                if (value != null)
                {
                    httpListenerContext.Response.ContentEncoding = value;
                    try
                    {
                        if (response != null)
                        {
                            responseBytes = httpListenerContext.Response.ContentEncoding.GetBytes(response);
                        }
                        else if (responseBytes != null)
                        {
                            response = httpListenerContext.Response.ContentEncoding.GetString(responseBytes);
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorOutput.WriteLine(e);
                    }
                }
            }
        }

        /// <summary>
        /// Status code
        /// </summary>
        public int StatusCode
        {
            get
            {
                return httpListenerContext.Response.StatusCode;
            }
            set
            {
                httpListenerContext.Response.StatusCode = value;
            }
        }

        /// <summary>
        /// Content type
        /// </summary>
        public string ContentType
        {
            get
            {
                return httpListenerContext.Response.ContentType;
            }
            set
            {
                if (value != null)
                {
                    httpListenerContext.Response.ContentType = value;
                }
            }
        }

        /// <summary>
        /// Standard output
        /// </summary>
        public TextWriter StandardOutput
        {
            get
            {
                if (standardOutput == null)
                {
                    standardOutput = Console.Out;
                }
                return standardOutput;
            }
        }

        /// <summary>
        /// Standard error output
        /// </summary>
        public TextWriter ErrorOutput
        {
            get
            {
                if (errorOutput == null)
                {
                    errorOutput = Console.Error;
                }
                return errorOutput;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpListenerContext">HTTP listener context</param>
        public ListenerContext(HttpListenerContext httpListenerContext)
        {
            this.httpListenerContext = httpListenerContext;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpListenerContext">HTTP listener context</param>
        /// <param name="standardOutput">Standard output</param>
        /// <param name="errorOutput">Error output</param>
        public ListenerContext(HttpListenerContext httpListenerContext, TextWriter standardOutput, TextWriter errorOutput)
        {
            this.httpListenerContext = httpListenerContext;
            this.standardOutput = standardOutput;
            this.errorOutput = errorOutput;
        }
    }
}
