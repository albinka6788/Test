using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using BHIC.Domain;
using BHIC.Common.XmlHelper;
using BHIC.Domain.FileType;
using System.Text.RegularExpressions;

namespace BHIC.Common.CommonUtilities
{
    public class UploadHelper
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Validate the uploading files
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="outPutFileFullName"></param>
        /// <param name="zippedFileName"></param>
        /// <returns></returns>
        public static FileUploadValidationStaus ValidateUploadedFiles(HttpPostedFileBase selectedFiles, string outPutFileFullName, string zippedFileName = "")
        {
            return ValidateUploadedFiles(selectedFiles.InputStream, outPutFileFullName, zippedFileName);
        }

        /// <summary>
        /// Validate the uploading stream
        /// </summary>
        /// <param name="selectedFiles"></param>
        /// <param name="outPutFileFullName"></param>
        /// <param name="zippedFileName"></param>
        /// <returns></returns>
        public static FileUploadValidationStaus ValidateUploadedFiles(Stream selectedFiles, string outPutFileFullName, string zippedFileName = "")
        {
            try
            {
                FileInfo info = new FileInfo(outPutFileFullName);
                FileInfo zippedFileInfo = null;
                if (!string.IsNullOrEmpty(zippedFileName))
                {
                    zippedFileInfo = new FileInfo(zippedFileName);
                }

                if (info.Exists)
                {
                    return new FileUploadValidationStaus
                    {
                        IsValid = false,
                        ValidationMessages = string.Format("Specified file '{0}' already exists", Path.GetFileName(outPutFileFullName))
                    };
                }
                else if (selectedFiles.Length < ConfigCommonKeyReader.MinFileSize)
                {
                    return new FileUploadValidationStaus
                    {
                        IsValid = false,
                        ValidationMessages = string.Format("The minimum allowed file size is {0} KB.", ConfigCommonKeyReader.MinFileSize)
                    };
                }
                else if (selectedFiles.Length >= ConfigCommonKeyReader.MaxFileSize)
                {
                    return new FileUploadValidationStaus
                    {
                        IsValid = false,
                        ValidationMessages = string.Format("The maximum allowed file size is {0} KB", ConfigCommonKeyReader.MaxFileSize)
                    };
                }
                else if (!ConfigCommonKeyReader.AllowedFileTypes.Contains(info.Extension.ToLowerInvariant()))
                {
                    return new FileUploadValidationStaus
                    {
                        IsValid = false,
                        ValidationMessages = "Specified file type is not supported"
                    };
                }
                else if (!string.IsNullOrEmpty(zippedFileName) && !ConfigCommonKeyReader.AllowedFileTypes.Contains(zippedFileInfo.Extension.ToLowerInvariant()))
                {
                    return new FileUploadValidationStaus
                    {
                        IsValid = false,
                        ValidationMessages = "Zipped file contains non supported file type"
                    };
                }
                else if (info.Name.Length > ConfigCommonKeyReader.MaxFileNameChar)
                {
                    return new FileUploadValidationStaus
                    {
                        IsValid = false,
                        ValidationMessages = string.Format("The maximum file name length is {0} characters. Please rename your file and attempt to upload the file again.", ConfigCommonKeyReader.MaxFileNameChar)
                    };
                }
                //else if (info.Name.Length < ConfigCommonKeyReader.MaxFileNameChar)
                //{
                //    Regex ItemRegex = new Regex("^[a-zA-Z0-9_@(+).,-]+$");
                //    foreach (Match ItemMatch in ItemRegex.Matches(info.Name))
                //    {
                //        if (!ItemMatch.Success)
                //        {
                //            return new FileUploadValidationStaus
                //            {
                //                IsValid = false,
                //                ValidationMessages = "The file name has invalid characters. Please rename your file and attempt to upload the file again."
                //            };
                //        }
                //    }

                //}
                else
                {
                    FileType fileType = selectedFiles.GetFileType();
                    if (fileType != null && !string.IsNullOrEmpty(fileType.Extension) && !ConfigCommonKeyReader.AllowedFileTypes.Any(res => res.Equals(fileType.ToString(), StringComparison.OrdinalIgnoreCase)))
                    {
                        return new FileUploadValidationStaus
                        {
                            IsValid = false,
                            ValidationMessages = "Specified file type is not supported"
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                return new FileUploadValidationStaus
                {
                    IsValid = false,
                    ValidationMessages = ex.Message
                };
            }
            return new FileUploadValidationStaus
            {
                IsValid = true
            };
        }

        #endregion

        #endregion
    }

    public class FileUploadValidationStaus
    {
        public bool IsValid { get; set; }
        public string ValidationMessages { get; set; }
    }

}
