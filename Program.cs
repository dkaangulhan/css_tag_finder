using System;
using System.Collections.Generic;
using System.IO;
namespace css
{
    class Program
    {
        //Path to read the content
        static string fileToRead = "";

        //Path that updated content will be written to
        static string pathToCreateFile = "";

        static void Main(string[] args)
        {
            CSSInterpreter cssInterpreter = new CSSInterpreter(pathToCreateFile);
            
            while(cssInterpreter.ui());



            Console.WriteLine("Exiting Program");
            Console.ReadLine();
        }

        public static void cssInterpret()
        {
            string str = "";

            while (str != "qt")
            {
                Console.WriteLine("New operation : new \t Exit : qt");
                str = Console.ReadLine();
                if (str == "new")
                {
                    CSSInterpreter interpreter = new CSSInterpreter(pathToCreateFile);
                    bool kontrol = true;
                    while (kontrol)
                    {
                        kontrol = interpreter.ui();
                    }
                }
                else if (str == "qt")
                {
                    Console.WriteLine("Exiting..");
                }
            }


        }

    }

    public class CSSInterpreter
    {
        public List<CSSElement> cssElementList = new List<CSSElement>();
        public string cssContent;
        public string pathToCreateFile;

        public CSSInterpreter(string pathToCreateFile)
        {
            this.pathToCreateFile = pathToCreateFile; 
        }

        public void tagFinder()
        {
            Console.WriteLine("Location of file :");
            string location = Console.ReadLine();
            FileStream fs = new FileStream(location, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            cssContent = sr.ReadToEnd();
            fs.Flush();
            fs.Close();

            int errorChecker = 0; //Increased when adding tag - Decreased when removing tag

            bool isThereProblem = false;
            CSSElement currentOwnerElement = null;
            for (int i = 0; i < cssContent.Length; i++)
            {
                if (cssContent[i] == '<')
                {
                    if (i + 1 < cssContent.Length && cssContent[i + 1] == '/')
                    {
                        int indexOfChar = findIndexOfLetter(cssContent, i + 1, '>');
                        string tag = textExtracter(cssContent, i + 2, indexOfChar);
                        bool tagFound = false;
                        for (int p = cssElementList.Count - 1; p >= 0; p--)
                        {
                            if (cssElementList[p].cssTag == tag)
                            {
                                if (cssElementList[p].tagClosed == false)
                                {
                                    cssElementList[p].tagClosed = true;
                                    cssElementList[p].endingLine = i - 1;
                                    if (cssElementList[p].ownerTag != null)
                                    {
                                        currentOwnerElement = cssElementList[p].ownerTag;
                                    }
                                    else
                                    {
                                        currentOwnerElement = null;
                                    }
                                    tagFound = true;
                                    errorChecker--;
                                    break;
                                }
                            }
                        }
                        if (!tagFound)
                        {
                            isThereProblem = true;
                            Console.WriteLine("Tag Couldn't Get Closed : " + tag);
                        }
                        if (isThereProblem)
                        {
                            break;
                        }
                        else
                        {
                            i = indexOfChar;
                        }
                    }
                    else
                    {
                        string tagName = "";
                        int propertyStartingLine = i;
                        i++;
                        while (i < cssContent.Length && cssContent[i] != ' ' && cssContent[i] != '>')
                        {
                            tagName += cssContent[i];
                            i++;
                        }
                        if (tagName != "img")
                        {
                            if (i < cssContent.Length)
                            {
                                int index = -1;
                                if (cssContent[i] == '>')
                                {
                                    index = i;
                                }
                                else if (cssContent[i] == ' ')
                                {
                                    //<tag></tag> this type of tags can cause an error of that endingLine is lower than startingLine ! might cause an error
                                    index = findIndexOfLetter(cssContent, i, '>');
                                    i = index;
                                }
                                CSSElement element = new CSSElement(tagName, index + 1, -1, propertyStartingLine, index);
                                cssElementList.Add(element);
                                if (currentOwnerElement != null)
                                {
                                    element.ownerTag = currentOwnerElement;
                                    currentOwnerElement.innerTagList.Add(element);
                                }
                                currentOwnerElement = element;
                                errorChecker++;
                            }
                        }
                    }
                }
            }

            if (errorChecker != 0)
            {
                isThereProblem = true;
                Console.WriteLine("Error Checker Problem -> " + errorChecker);
            }
            if (isThereProblem)
            {
                Console.WriteLine("Error Happened");
            }
            //Console.WriteLine("Tags added");
        }
        public void tagFinder(string location)
        {
            FileStream fs = new FileStream(location, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            cssContent = sr.ReadToEnd();
            fs.Flush();
            fs.Close();

            int errorChecker = 0; //Increased when adding tag - Decreased when removing tag

            bool isThereProblem = false;
            CSSElement currentOwnerElement = null;
            for (int i = 0; i < cssContent.Length; i++)
            {
                if (cssContent[i] == '<')
                {
                    if (i + 1 < cssContent.Length && cssContent[i + 1] == '/')
                    {
                        int indexOfChar = findIndexOfLetter(cssContent, i + 1, '>');
                        string tag = textExtracter(cssContent, i + 2, indexOfChar);
                        bool tagFound = false;
                        for (int p = cssElementList.Count - 1; p >= 0; p--)
                        {
                            if (cssElementList[p].cssTag == tag)
                            {
                                if (cssElementList[p].tagClosed == false)
                                {
                                    cssElementList[p].tagClosed = true;
                                    cssElementList[p].endingLine = i - 1;
                                    if (cssElementList[p].ownerTag != null)
                                    {
                                        currentOwnerElement = cssElementList[p].ownerTag;
                                    }
                                    else
                                    {
                                        currentOwnerElement = null;
                                    }
                                    tagFound = true;
                                    errorChecker--;
                                    break;
                                }
                            }
                        }
                        if (!tagFound)
                        {
                            isThereProblem = true;
                            Console.WriteLine("Tag Couldn't Get Closed : " + tag);
                        }
                        if (isThereProblem)
                        {
                            break;
                        }
                        else
                        {
                            i = indexOfChar;
                        }
                    }
                    else
                    {
                        string tagName = "";
                        int propertyStartingLine = i;
                        i++;
                        while (i < cssContent.Length && cssContent[i] != ' ' && cssContent[i] != '>')
                        {
                            tagName += cssContent[i];
                            i++;
                        }
                        if (tagName != "img")
                        {
                            if (i < cssContent.Length)
                            {
                                int index = -1;
                                if (cssContent[i] == '>')
                                {
                                    index = i;
                                }
                                else if (cssContent[i] == ' ')
                                {
                                    //<tag></tag> this type of tags can cause an error of that endingLine is lower than startingLine ! might cause an error
                                    index = findIndexOfLetter(cssContent, i, '>');
                                    i = index;
                                }
                                CSSElement element = new CSSElement(tagName, index + 1, -1, propertyStartingLine, index);
                                cssElementList.Add(element);
                                if (currentOwnerElement != null)
                                {
                                    element.ownerTag = currentOwnerElement;
                                    currentOwnerElement.innerTagList.Add(element);
                                }
                                currentOwnerElement = element;
                                errorChecker++;
                            }
                        }
                    }
                }
            }

            if (errorChecker != 0)
            {
                isThereProblem = true;
                //Console.WriteLine("Error Checker Problem -> " + errorChecker);
            }
            if (isThereProblem)
            {
                //Console.WriteLine("Error Happened");
            }
            //Console.WriteLine("Tags added");
        }

        public int findIndexOfLetter(string content, int start, char wantedChar)
        {
            //There can be a problem of that if there  is some random letter between tag name and '>'
            //this method will count the letters between tag and '>'. This should be avoided
            int index = -1;
            for (int i = start + 1; i < content.Length; i++)
            {
                if (content[i] == wantedChar)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public string textExtracter(string content, int start, int end)
        {
            if (end > content.Length)
            {
                end = content.Length;
            }
            string str = "";
            for (int i = start; i < end; i++)
            {
                str += content[i];
            }
            return str;
        }

        public void contentFinder()
        {
            Console.WriteLine("Content Finder -> Wanted Tag ?");
            string wantedTag = Console.ReadLine();
            Console.WriteLine();
            for (int i = 0; i < cssElementList.Count; i++)
            {
                if (cssElementList[i].cssTag == wantedTag)
                {
                    string text = textExtracter(cssContent, cssElementList[i].startingLine, cssElementList[i].endingLine + 1);
                    Console.WriteLine(text);
                    Console.WriteLine("<->");
                }
            }
        }

        /*
         *This finds tags which are sub-tags of headTags
         */
        public void intervalWriter()
        {
            Console.WriteLine("Head Tag?");
            string headTag = Console.ReadLine();

            List<string> wantedTags = new List<string>();//Tags that should be in headTag's field
            string wanted = "";
            while (wanted != "qt")
            {
                Console.WriteLine("Wanted tags?(//qt for exit)");
                wanted = Console.ReadLine();
                wantedTags.Add(wanted);
            }

            for (int i = 0; i < cssElementList.Count; i++)
            {
                if (cssElementList[i].cssTag == headTag)
                {
                    int inputIndex = i;
                    i++;
                    while (i < cssElementList.Count && cssElementList[inputIndex].endingLine > cssElementList[i].endingLine)
                    {
                        if (cssElementList[i].innerTagList.Count == 0 && wantedTags.Contains(cssElementList[i].cssTag) || wantedTags.Contains("*"))
                        {
                            Console.WriteLine(emptyLetterRemover(textExtracter(cssContent, cssElementList[i].startingLine, cssElementList[i].endingLine + 1)));
                        }
                        i++;
                    }
                    i--;
                    Console.WriteLine("\n<----------->\n");
                }
            }

        }

        public void intervalWriterNotSubTags()
        {
            List<string> wantedTags = new List<string>();
            string wanted = "";
            while (wanted != "qt")
            {
                Console.WriteLine("Wanted tags?(//qt for exit)");
                wanted = Console.ReadLine();
                wantedTags.Add(wanted);
            }

            for (int i = 0; i < cssElementList.Count; i++)
            {
                if (cssElementList[i].innerTagList.Count == 0 && wantedTags.Contains(cssElementList[i].cssTag) || wantedTags.Contains("*"))
                {
                    Console.WriteLine(emptyLetterRemover(textExtracter(cssContent, cssElementList[i].startingLine, cssElementList[i].endingLine + 1)));
                    Console.WriteLine("\n<----------->\n");

                }
            }

        }

        public string emptyLetterRemover(string content, int start, int end)
        {
            string text = "";
            for (int i = start; i < end; i++)
            {
                if (content[i] != ' ' || content[i] != '\n')
                {
                    text += content[i];
                }
            }
            return text;
        }

        public string emptyLetterRemover(string content)
        {
            string text = "";
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] != ' ' && content[i] != '\n' && content[i] != '\t' && content[i] != '\r')
                {
                    text += content[i];
                }
            }
            return text;
        }

        public string emptyLetterRemover(string content, char appendInstead)
        {
            string text = "";
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] != ' ' && content[i] != '\n' && content[i] != '\t' && content[i] != '\r')
                {
                    text += content[i];
                }
                else
                {
                    text += appendInstead;
                }
            }
            return text;
        }

        /*
         *Finds properties inside wanted tags
         */
        public void propertyFinder()
        {
            List<string> wantedTags = new List<string>();//Tags that should be in headTag's field
            string wanted = "";
            while (wanted != "qt")
            {
                Console.WriteLine("Wanted tags?(//qt for exit)");
                wanted = Console.ReadLine();
                wantedTags.Add(wanted);
            }

            List<string> wantedProperties = new List<string>();//Tags that should be in headTag's field
            string wantedProperty = "";
            while (wantedProperty != "qt")
            {
                Console.WriteLine("Wanted Properties?(//qt for exit)");
                wantedProperty = Console.ReadLine();
                wantedProperties.Add(wantedProperty);
            }


            for (int i = 0; i < cssElementList.Count; i++)
            {
                if (wantedTags.Contains(cssElementList[i].cssTag))
                {
                    for (int k = 0; k < wantedProperties.Count; k++)
                    {
                        Console.WriteLine(wordFinder(cssContent, cssElementList[i].propertyStartingLine, cssElementList[i].propertyEndingLine, wantedProperties[k]));
                    }
                }
            }

        }

        public List<string> propertyFinderGetter(List<string> wantedTags, List<string> wantedProperties)
        {
            List<string> returningList = new List<string>();

            for (int i = 0; i < cssElementList.Count; i++)
            {
                if (wantedTags.Contains(cssElementList[i].cssTag))
                {
                    for (int k = 0; k < wantedProperties.Count; k++)
                    {
                        string property = wordFinder(cssContent, cssElementList[i].propertyStartingLine, cssElementList[i].propertyEndingLine, wantedProperties[k]);

                        if (property != "" && isThereAlready(property, returningList))
                        {
                            returningList.Add(property);
                        }
                    }
                }
            }

            //Console.WriteLine("Properties found..");

            return returningList;
        }

        public bool isThereAlready(string stringToBeControlled, List<string> textString)
        {
            //if there is same text return false
            for (int i = 0; i < textString.Count; i++)
            {
                if (stringToBeControlled == textString[i])
                {
                    return false;
                }
            }
            return true;
        }

        public string wordFinder(string content, int start, int end, string wantedWord)
        {
            string text = "";
            for (int i = start; i < end; i++)
            {
                if (content[i] == wantedWord[0])
                {
                    bool correct = true;
                    for (int k = 1; k < wantedWord.Length; k++)
                    {
                        i++;
                        if (wantedWord[k] != content[i])
                        {
                            correct = false;
                            break;
                        }
                    }
                    if (correct)
                    {
                        int equalSignStart = findIndexOfLetter(content, i, '"');
                        int equalSignEnd = findIndexOfLetter(content, equalSignStart, '"');
                        text = textExtracter(content, equalSignStart + 1, equalSignEnd);
                        break;
                    }
                }
            }
            return text;
        }

        public void fileCreator(string tagName)
        {
            for (int i = 0; i < cssElementList.Count; i++)
            {
                if (cssElementList[i].cssTag == tagName)
                {
                    FileStream fs = new FileStream(pathToCreateFile, FileMode.OpenOrCreate);
                    int tagClosing = 4 + cssElementList[i].cssTag.Length;
                    byte[] bits = new byte[(cssElementList[i].endingLine - cssElementList[i].propertyStartingLine) + tagClosing];
                    for (int k = 0; k < bits.Length - tagClosing; k++)
                    {
                        bits[k] = Convert.ToByte(cssContent[cssElementList[i].propertyStartingLine + k]);
                    }
                    string str = "></" + cssElementList[i].cssTag + ">";
                    int index = 0;
                    for (int k = bits.Length - tagClosing; k < bits.Length; k++)
                    {
                        bits[k] = Convert.ToByte(str[index]);
                        index++;
                    }

                    fs.Write(bits, 0, bits.Length);
                    fs.Flush();
                    fs.Close();
                    Console.WriteLine("File Created..");
                    break;
                }
            }
        }

        public bool ui()
        {
            string methodList = "Tag Finder : T - FileCreator: F - Interval Writer : IW - Interval Writer Without Sub Tags : IWNS \n" +
                "Property Finder : PF - Exit : qt";
            Console.WriteLine("Choose a method -> \n" + methodList);
            string method = "";
            method = Console.ReadLine();

            if (method == "T")
            {
                tagFinder();
                Console.WriteLine("Tags found..");
            }
            else if (method == "F")
            {
                string tagName = "";
                string fileName = "";

                Console.WriteLine("Which tag do you want to write to a file?");
                tagName = Console.ReadLine();

                Console.WriteLine("What will be the name of the file?");
                fileName = Console.ReadLine();

                fileCreator(tagName);
            }
            else if (method == "IW")
            {
                intervalWriter();
            }
            else if (method == "IWNS")
            {
                intervalWriterNotSubTags();
            }
            else if (method == "PF")
            {
                propertyFinder();
            }
            else if (method == "qt")
            {
                Console.WriteLine("Quitting");
                return false;
            }
            else
            {
                Console.WriteLine("Wrong input.. \n\n");
            }

            return true;
        }

    }

    public class CSSElement
    {
        public string cssTag;
        public bool tagClosed;
        public int startingLine;
        public int endingLine;
        public int propertyStartingLine;//This is used for where tag is started. So properties inside the tag can be found easily 
        public int propertyEndingLine;
        public CSSElement ownerTag;
        public List<CSSElement> innerTagList;
        public CSSElement(string cssTag, int startingLine, int endingLine, int propertyStartingLine, int propertyEndingLine)
        {
            this.cssTag = cssTag;
            this.startingLine = startingLine;
            this.endingLine = endingLine;
            this.propertyStartingLine = propertyStartingLine;
            this.propertyEndingLine = propertyEndingLine;
            innerTagList = new List<CSSElement>();
        }
    }

}