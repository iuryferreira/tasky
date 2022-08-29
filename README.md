<p align="center">
    <img alt="Tasky" title="Tasky" src=".github/images/logo.svg" width="400px" />
</p>
<h3 align="center">
    Manage your tasks via the command line
</h3>

<p align="center">
    <a href="#">
        <img alt="Version" src="https://img.shields.io/github/v/release/iuryferreira/tasky?include_prereleases&label=latest%20version">
    </a>
    <a href="#">
        <img alt="Downloads" src="https://img.shields.io/github/downloads/iuryferreira/tasky/total">
    </a>
    <a href="https://dotnet.microsoft.com/">
        <img alt="GitHub language count" src="https://img.shields.io/badge/language-C%23-blue">
    </a>
    <a href="https://github.com/iuryferreira/">
        <img alt="Made by Iury Ferreira" src="https://img.shields.io/badge/made%20by-Iury%20Ferreira-blue">
    </a>


</p>


<p align="center" style="font-size:10px">Não fala inglês? veja a documentação em português <a href="README.md">aqui</a>.</p>

<br>
Tasky is a task manager made for command lines, bringing the possibility for the user to organize himself in a simple way just using his terminal and a keyboard.
It is cross-platform and can be used both on Windows with Poweshell/CMD or on linux with Bash/Fish/Zsh among others.


### Usage

After having downloaded and executed the installation script, just open the terminal and execute the application commands, 
which can be consulted with the command `tasky --help`. 

After downloading and executing the installation script, just open the terminal and execute the application's commands, which can be consulted with the command tasky --help. Through the commands you will be able to add, remove, start, complete, change priority and edit your tasks. 
We have complete management from the command line. 

#### Add

To add a task you need to run the `add` command. You will also need to inform the name of the board that your task will be inserted, and if it doesn't exist, it will be created automatically.
See the example below:

```powershell
tasky add "buy fruit at the market" --board "daily"
```
The terminal will display:

![Add Output](.github/images/examples/add_01.png)

##### Priority

You can also set a priority for your task when you try to add it. It can be normal (default), medium and high. See the example below:

```powershell
tasky add "buy fruit at the market" --board "daily" --priority high
```
The terminal will display:

![Add Output](.github/images/examples/add_02.png)

High priority is in red and medium is in yellow.


### Improvements:

- Undo e redo last operation
- Log operations