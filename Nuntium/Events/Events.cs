﻿
using Nuntium.Core;
using Prism.Events;

namespace Nuntium
{
    public class ToggleSearchSectionVisibilityEvent : PubSubEvent { }

    public class LineSpacingChangedEvent : PubSubEvent<double> { }

    public class EmailDeletedEvent : PubSubEvent<string> { }

    public class EmailStaredEvent : PubSubEvent<string> { }

    public class EmailArchivedEvent : PubSubEvent<string> { }

    public class MarkEmailAsUnread : PubSubEvent<string> { }

    public class MoveEmailToCatalog : PubSubEvent<EmailCatalogPayload> { }


}
