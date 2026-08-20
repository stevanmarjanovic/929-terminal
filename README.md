# 929 Learning Program CLI

<img width="751" height="544" alt="Screenshot 2026-08-20 at 16 13 09" src="https://github.com/user-attachments/assets/98e7638a-361c-4f14-a1fe-dffe6b47d74d" />

The 929 Tanakh learning program invites everyone to read one chapter at a time, in order, as part of a shared journey through all 929 chapters. This CLI brings the current chapter and its available summary directly to your terminal.

## Prerequisites

The project targets .NET 9 and is published as a Native AOT executable. You need the .NET 9 SDK to build it. Native AOT builds are platform-specific, so publish it on (or for) the platform where you plan to run it.

## Usage

```shell
# Show the daily chapter and its summary, if available
929-cli

# Remove the link below the summary
929-cli --no-link

# Display the title without box borders
929-cli --simple
```

## Building

Publish the app as a Native AOT executable for your platform:

`dotnet publish -c Release -r osx-arm64`

This generates the executable in `./bin/Release/net9.0/osx-arm64/publish`.
On macOS, a `.dSYM` file may also be generated; it is used for debugging and is not required to run the CLI.

Replace `osx-arm64` with the runtime identifier for your target platform, such as `linux-x64` or `win-x64`.

## Deployment

Copy the published executable to a directory on your `PATH`. For example, on macOS or Linux:

```shell
mkdir -p ~/.local/bin
cp ./bin/Release/net9.0/osx-arm64/publish/929-cli ~/.local/bin/929-cli
```

If `~/.local/bin` is not already on your `PATH`, add it to your shell configuration before using the command.

## Terminal startup

To show the daily reading automatically whenever an interactive terminal session starts, add `929-cli` to your shell's startup file.

For zsh, add this line to `~/.zshrc`:

```bash
929-cli
```

If `~/.local/bin` is not already on your `PATH`, add this before the command:

```bash
export PATH="$HOME/.local/bin:$PATH"
```

Then reload your shell configuration or open a new terminal:

```shell
source ~/.zshrc
```

If you use a different shell, add `929-cli` to its interactive startup file, such as `~/.bashrc` for Bash.

## Credits

Summaries in `summaries.json` were fetched from the OpenScripture API (snapshot from July 2025).
