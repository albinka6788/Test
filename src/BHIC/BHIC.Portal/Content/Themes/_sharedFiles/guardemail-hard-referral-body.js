﻿<!--

The following variables can optionally be included in the associated email message below.  Place the variables (including #'s) anywhere within valid html markup below.

	####MGACode##, ##GovState##, ##EffDate##, ##Premium##, 
	##ContactName##, ##ContactPhone##, ##BusinessName##, ##MailAddr1##, ##MailAddr2##, ##MailCity##, ##MailState##, ##MailZip##, ##ContactEmail##, 
	
	##DecisionStatus##, ##DecisionStatusDescription##, ##ImportStatus##, ##QuoteId##

Up to 5 sets of class information variables are also available as follows; reference the class number (1 - 5) in the variable name as needed:

	##ClassState1##, ##ClassCode1##, ##ClassSuffix1##, ##ClassPayroll1##,

-->

<style>
	.dsInfoTable {
	    border-style: solid;
	    border-width: thin;
	    border-color: black;
	    width: 100%;
	}
</style>

<p>
	<b>Action Required: Hard Referral from Direct Sales System</b>
	<br />
	The quote below was generated by GUARD's Direct Sales system, and a corresponding prospect has been created and is available for review via the ASC.
	<br />
	(See the glossary at the end of this message for a definition of "Hard Referral")
</p>

<p>
	<b>Additional Instructions</b>
	<br />
	Step 1: Quick Search – Is it possible this may be underwriting-eligible?  Consult Work Flow Outline, NLF UW Appetite, and brief Google search.
	<br />
	Step 2: Enter the lead in Epic, and pay close attention to coding: Agy = 018  /  Branch = 300  /  Lead Source = NLF – Direct Sales
	<br />
	Step 3: Contact the prospect as they requested, and proceed according to potential for underwriting success.  Consult Work Flow Outline.
</p>


<b>Policy Information</b>
<br />
<table class="dsInfoTable">
	<tr>
		<td><b>Business Name:</b></td>
		<td>##BusinessName##</td>
	</tr>
	<tr>
		<td><b>MGA Code:</b></td>
		<td>##MGACode##</td>
	</tr>
	<tr>
		<td><b>Governing State:</b></td>
		<td>##GovState##</td>
	</tr>
	<tr>
		<td><b>Effective Date:</b></td>
		<td>##EffDate##</td>
	</tr>
	<tr>
		<td><b>Premium:</b></td>
		<td>##Premium##</td>
	</tr>
</table>
<br />


<b>Quote Status</b>
<br />
<table class="dsInfoTable">
	<tr>
		<td><b>Decision Status:</b></td>
		<td>##DecisionStatus##</td>
	</tr>
	<tr>
		<td><b>Decision Description</b></td>
		<td>##DecisionStatusDescription##</td>
	</tr>
	<tr>
		<td><b>Import Status:</b></td>
		<td>##ImportStatus##</td>
	</tr>
	<tr>
		<td><b>Quote ID:</b></td>
		<td>##QuoteId##</td>
	</tr>
</table>
<br />


<b>General Policy Information</b>
<br />
<table class="dsInfoTable">
	<tr>
		<td><b>Business Name:</b></td>
		<td>##BusinessName##</td>
	</tr>
	<tr>
		<td><b>Governing State:</b></td>
		<td>##GovState##</td>
	</tr>
	<tr>
		<td><b>Effective Date:</b></td>
		<td>##EffDate##</td>
	</tr>
</table>
<br />

<b>Class Information</b>
<br />
<table class="dsInfoTable">

	<tr>
		<td><b>State</b></td>
		<td><b>Class Code</b></td>
		<td><b>Class Suffix</b></td>
		<td><b>Annual Payroll</b></td>
	</tr>

	<tr>
		<td>##ClassState1##</td>
		<td>##ClassCode1##</td>
		<td>##ClassSuffix1##</td>
		<td>##ClassPayroll1##</td>
	</tr>

	<tr>
		<td>##ClassState2##</td>
		<td>##ClassCode2##</td>
		<td>##ClassSuffix2##</td>
		<td>##ClassPayroll2##</td>
	</tr>

	<tr>
		<td>##ClassState3##</td>
		<td>##ClassCode3##</td>
		<td>##ClassSuffix3##</td>
		<td>##ClassPayroll3##</td>
	</tr>

	<tr>
		<td>##ClassState4##</td>
		<td>##ClassCode4##</td>
		<td>##ClassSuffix4##</td>
		<td>##ClassPayroll4##</td>
	</tr>

	<tr>
		<td>##ClassState5##</td>
		<td>##ClassCode5##</td>
		<td>##ClassSuffix5##</td>
		<td>##ClassPayroll5##</td>
	</tr>

</table>
<span>*A maximum of 5 classes will be listed above</span>
<br />
<br />

<b>Contact Information</b>
<br />
<table class="dsInfoTable">
	<tr>
		<td><b>Name:</b></td>
		<td>##ContactName##</td>
	</tr>
	<tr>
		<td><b>Phone:</b></td>
		<td>##ContactPhone##</td>
	</tr>
	<tr>
		<td><b>Email:</b></td>
		<td>##ContactEmail##</td>
	</tr>
</table>
<br />

<b>Mailing Address</b>
<br />
<table class="dsInfoTable">
	<tr>
		<td><b>Address Line 1:</b></td>
		<td>##MailAddr1##</td>
	</tr>
	<tr>
		<td><b>Address Line 2:</b></td>
		<td>##MailAddr2##</td>
	</tr>
	<tr>
		<td><b>City:</b></td>
		<td>##MailCity##</td>
	</tr>
	<tr>
		<td><b>State:</b></td>
		<td>##MailState##</td>
	</tr>
	<tr>
		<td><b>Zip:</b></td>
		<td>##MailZip##</td>
	</tr>
</table>
<br />

<p>
	<b>Explanation of terms used in this document:</b>
	<ul>
		<li>
			<u>Hard Referral:</u> A prospect was created on GUARD systems, and is available for review (an MGA code should be available). The user has been advised that their request has been referred to NL&F's underwriting department, and that they will be contacted within one business day.
		</li>
		<li>
			<u>Soft Referral:</u> Information related to a prospect is contained in this email.  The prospect likely doesn't meet typical GUARD business standards for policy issuance, and policy information has not been added to GUARD systems.  The user has been advised that their request has been referred to NL&F's underwriting department, and that they will be contacted within one business day.
		</li>
		<li>
			<u>Issued Policy:</u> A policy has been issued on GUARD systems, and is available for review (an MGA code shoud be available). The user has access to their policy information via the Direct Sales site.
		</li>
		<li>
			<u>Decision Status:</u> Reflects the outcome of an automated decision process which assesses user responses to questions, and other minimum requirements associated with policy issuance.  Example status messages: 'Hard Referral', 'Soft Referral', 'Issue Policy'.
		</li>
		<li>
			<u>Import Status:</u> Indicates whether or not a prospect or issued policy was imported from the Direct Sales system to IIS or ASC.
		</li>
	</ul>
</p>

<p>
	<b>CONFIDENTIALITY NOTICE</b>
	<br />
	THIS E-MAIL, INCLUDING ANY ATTACHED FILES, MAY CONTAIN CONFIDENTIAL, PROPRIETARY AND/OR PRIVILEGED INFORMATION FOR THE SOLE USE OF THE INTENDED RECIPIENT(S). ANY REVIEW, USE, DISTRIBUTION, COPY OR DISCLOSURE BY OTHERS IS STRICTLY PROHIBITED. IF YOU ARE NOT THE INTENDED RECIPIENT (OR AUTHORIZED TO RECEIVE INFORMATION FOR THE RECIPIENT), PLEASE CONTACT THE SENDER AND DELETE ALL COPIES OF THIS MESSAGE.

	THANK YOU.
</p>
