# Mashup-based compliance checking (MCC) engine

This is a version of MCC engine built on C# and .NET. It relies on a fork of the 
library Styx (also included in the project) that provides a dataflow foundation.

The MCC engine adds three things on top of Styx:
- A mechanism to define compliance controls, link them with mashups and enable
their automated evaluation.
- A mechanism to build parameterized mashups from a generic description of them.
This allows to build graphical editors to create mashups.
- An extended set of generic pipes that can be used to create mashups.

This MCC engine is the open source version of the core solution that was used
in several compliance checking projects with different organizations. In those
projects several closed source implementations were developed. Some examples 
are:
- An editor based on Enteprise Architect that enables the graphical definition
of mashups. 
- A way to specify compliance controls by means of an Excel file.
- Many domain-specific pipes that enables the interaction of the MCC engine with
the information system of the organization like Microsoft Sharepoint.

## Use

The MCC engine is not supposed to be used as is, but it needs to be extended to
build a complete compliance checking solution. Specifically, five things need 
to be done to do so.

1. Implement the domain-specific pipes that are necessary for the compliance
checking solution. These pipes must implement interface `IPipe` and must be
encapsulated into one or several assemblies so that they can be loaded 
afterwards. There are many examples of pipes in 
[Pipes Generic](MCC/Pipes.Generic) or in [Styx Pipes](Styx/Pipes). 

2. Implement interface [`IMashupRepository`](MCC/Mashups/IMashupRepository) 
to provide a way to access the Mashup definitions and the set of assemblies that 
include the domain-specific pipes. There are two assemblies that are loaded by 
default: `MCC.dll` and `Styx.dll`.

3. Provide a mechanism to load the information about the controls. This 
information must include a name, a description, and a `ControlInformation` that
include the mashups that implement the control and a context information.

4. Decide on how the evaluation of the controls should look like. MCC engine
provides a default instantiation of `IControl` called 
`IControlContextEvaluation` that uses the concept of context to provide 
additional information to the output of the mashup. However, MCC engine allows 
one to define their own implementation of `IControl` so that each solution can 
customize the way the information obtained from the mashups are processed and 
integrated.

5. Provide a mechanism to invoke the evaluation on each control and process the 
evaluation information obtained from them (e.g., using a graphical dashboard).

