#nullable enable
#pragma warning disable CS8618
namespace Module03_Async;
partial class Form1
{
    private System.ComponentModel.IContainer? components;
    private TabControl tabs;
    private TabPage danceTab, raceTab, fibTab, fileTab, folderTab;
    private NumericUpDown danceCount, fibLimit;
    private Button danceStart, raceStart, fibStart, fileBrowse, fileSearch, folderBrowse, folderSearch;
    private FlowLayoutPanel dancePanel;
    private ProgressBar[] horses = new ProgressBar[5];
    private Label raceResults;
    private TextBox fibOutput, filePath, fileWord, fileOutput, folderPath, folderWord, folderOutput;
    private ProgressBar[] danceBars = Array.Empty<ProgressBar>();
    protected override void Dispose(bool disposing){ if(disposing&&components!=null)components.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        Text="Многопоточность и асинхронность"; ClientSize=new Size(920,650); MinimumSize=new Size(800,560);
        tabs=new(){Dock=DockStyle.Fill}; Controls.Add(tabs);
        danceTab=new("Танцующие прогресс-бары"); raceTab=new("Скачки"); fibTab=new("Фибоначчи"); fileTab=new("Поиск в файле"); folderTab=new("Поиск в каталоге"); tabs.TabPages.AddRange(new[]{danceTab,raceTab,fibTab,fileTab,folderTab});
        danceTab.Controls.Add(new Label{Text="Количество индикаторов:",Left=20,Top=20,AutoSize=true}); danceCount=new(){Left=190,Top=16,Width=90,Minimum=1,Maximum=30,Value=8}; danceTab.Controls.Add(danceCount);
        danceStart=new(){Text="Старт",Left=300,Top=14,Width=100}; danceStart.Click+=async(_,_)=>await RunDanceAsync(); danceTab.Controls.Add(danceStart);
        dancePanel=new(){Left=20,Top=60,Width=850,Height=500,Anchor=AnchorStyles.Top|AnchorStyles.Bottom|AnchorStyles.Left|AnchorStyles.Right,FlowDirection=FlowDirection.TopDown,WrapContents=false,AutoScroll=true}; danceTab.Controls.Add(dancePanel);
        for(int i=0;i<5;i++){ horses[i]=new(){Left=25,Top=25+i*65,Width=820,Height=35,Maximum=100}; raceTab.Controls.Add(horses[i]); raceTab.Controls.Add(new Label{Text=$"Лошадь {i+1}",Left=25,Top=6+i*65,AutoSize=true}); }
        raceStart=new(){Text="Старт гонки",Left=25,Top=365,Width=130,Height=34}; raceStart.Click+=async(_,_)=>await RunRaceAsync(); raceTab.Controls.Add(raceStart);
        raceResults=new(){Left=25,Top=415,Width=820,Height=160,Anchor=AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Bottom,Text="Результаты появятся после финиша"}; raceTab.Controls.Add(raceResults);
        fibTab.Controls.Add(new Label{Text="Граница (все значения Фибоначчи от 0 до границы)",Left=20,Top=20,AutoSize=true}); fibLimit=new(){Left=380,Top=16,Width=160,Minimum=0,Maximum=1000000000000000000m,Value=1000000}; fibTab.Controls.Add(fibLimit);
        fibStart=new(){Text="Вычислить",Left=560,Top=14,Width=110}; fibStart.Click+=async(_,_)=>await RunFibAsync(); fibTab.Controls.Add(fibStart);
        fibOutput=new(){Left=20,Top=60,Width=850,Height=500,Multiline=true,ScrollBars=ScrollBars.Vertical,ReadOnly=true,Anchor=AnchorStyles.Top|AnchorStyles.Bottom|AnchorStyles.Left|AnchorStyles.Right}; fibTab.Controls.Add(fibOutput);
        MakeSearchTab(fileTab,false); MakeSearchTab(folderTab,true);
    }
    private void MakeSearchTab(TabPage tab,bool folder)
    {
        var path=new TextBox{Left=20,Top=25,Width=650,Anchor=AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right}; var browse=new Button{Text=folder?"Каталог...":"Файл...",Left=685,Top=23,Width=100,Anchor=AnchorStyles.Top|AnchorStyles.Right};
        var word=new TextBox{Left=100,Top=70,Width=250}; var search=new Button{Text="Найти",Left=370,Top=68,Width=100}; var output=new TextBox{Left=20,Top=115,Width=850,Height=440,Multiline=true,ScrollBars=ScrollBars.Vertical,ReadOnly=true,Anchor=AnchorStyles.Top|AnchorStyles.Bottom|AnchorStyles.Left|AnchorStyles.Right};
        tab.Controls.AddRange(new Control[]{new Label{Text=folder?"Папка:":"Путь:",Left=20,Top=6,AutoSize=true},path,browse,new Label{Text="Слово:",Left=20,Top=73,AutoSize=true},word,search,output});
        if(folder){folderPath=path;folderWord=word;folderOutput=output;folderBrowse=browse;folderSearch=search;browse.Click+=(_,_)=>BrowseFolder();search.Click+=async(_,_)=>await SearchFolderAsync();}
        else{filePath=path;fileWord=word;fileOutput=output;fileBrowse=browse;fileSearch=search;browse.Click+=(_,_)=>BrowseFile();search.Click+=async(_,_)=>await SearchFileAsync();}
    }
}
