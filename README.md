
# AnyStatus Kubernetes Plugin

[AnyStatus](https://www.anystat.us) plugin for Kubernetes.

Widgets for AnyStatus to provide capability to check various Kubernetes Cluster metrics.

[![Build status](https://ci.appveyor.com/api/projects/status/tt8r7y479u0rhxnu?svg=true)](https://ci.appveyor.com/project/fatihboy/anystatuskubernetes) [![NuGet](https://img.shields.io/nuget/v/AnyStatus.Plugins.Kubernetes.svg)](https://www.nuget.org/packages/AnyStatus.Plugins.Kubernetes/)

### Avaliable Widgets

-  [x] Namespace Count
-  [x] Pod Count

## Installation

In order to use Kubernetes plugin a you should create a readonly service account.

Create service account with following command;

    kubectl apply -f https://raw.githubusercontent.com/fatihboy/AnyStatusKubernetes/master/docs/setup/service-account.yaml

Read access token;

    kubectl get secrets -o jsonpath="{.items[?(@.metadata.annotations['kubernetes\.io/service-account\.name']=='anystatus-sa')].data.token}" | base64 -d

## Contribute

Contributions are most welcome :)
