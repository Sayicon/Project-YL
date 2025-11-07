/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   expansion_utils.c                                  :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: marcudos <marcudos@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/19 16:53:08 by marcudos          #+#    #+#             */
/*   Updated: 2025/06/01 18:08:13 by marcudos         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

void	add_token(t_args **head, char *value)
{
	t_args	*new;
	t_args	*temp;

	if (!value)
		return ;
	new = (t_args *)malloc(sizeof(t_args));
	if (!new)
		return ;
	new->arg = ft_strdup(value);
	new->next = NULL;
	if (!(*head))
		*head = new;
	else
	{
		temp = *head;
		while (temp->next)
			temp = temp->next;
		temp->next = new;
	}
}

void	add_token_free(t_args **head, char *value)
{
	add_token(head, value);
	free(value);
}

void	append_list(t_args **head, t_args *list)
{
	t_args	*temp;

	if (!list)
		return ;
	if (!(*head))
	{
		*head = list;
		return ;
	}
	temp = *head;
	while (temp->next)
		temp = temp->next;
	temp->next = list;
}

int	count_array(void **arr)
{
	int	i;

	i = 0;
	if (!arr)
		return (0);
	while (arr[i])
		i++;
	return (i);
}
