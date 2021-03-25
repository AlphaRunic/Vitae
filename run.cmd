@echo off
title Vitae REPL
cls

echo Building...
echo[
dotnet build
echo[
dotnet run --project ./vc/vc.csproj